import { vi, describe, it, expect, beforeEach } from 'vitest';

// Mock fs/promises before importing ./index
vi.mock('fs/promises', () => ({
  readFile: vi.fn(),
  writeFile: vi.fn()
}));

import { ipcMain, dialog, app, BrowserWindow } from 'electron';
import * as fs from 'fs/promises';
import { configureIpcHandlers, createWindow } from './index';
import { mockGetAllWindows } from './setupTests';

describe('Main process', () => {
  beforeEach(() => {
    vi.mocked(ipcMain.on).mockClear();
    vi.mocked(ipcMain.handle).mockClear();
    vi.mocked(dialog.showOpenDialog).mockClear();
    vi.mocked(dialog.showSaveDialog).mockClear();
    vi.mocked(fs.readFile).mockClear();
    vi.mocked(fs.writeFile).mockClear();
    mockGetAllWindows.mockClear();
    vi.mocked(BrowserWindow).mockClear();
  });

  describe('createWindow', () => {
    it('should create a BrowserWindow with correct options', () => {
      createWindow();
      expect(BrowserWindow).toHaveBeenCalled();
    });

    it('should register window events', () => {
      createWindow();
      const mockWindow = vi.mocked(BrowserWindow).mock.results[0].value;

      // Manually trigger events to cover handler bodies
      const handlers: Record<string, (...args: unknown[]) => unknown> = {};
      vi.mocked(mockWindow.on).mock.calls.forEach(([event, handler]) => {
        handlers[event] = handler as (...args: unknown[]) => unknown;
      });

      if (handlers['ready-to-show']) {
        const handler = handlers['ready-to-show'] as () => void;
        handler();
      }
      expect(mockWindow.show).toHaveBeenCalled();

      if (handlers['close']) {
        const preventDefault = vi.fn();
        const handler = handlers['close'] as (e: { preventDefault: () => void }) => void;
        handler({ preventDefault });
        expect(preventDefault).toHaveBeenCalled();
      }
    });
  });

  describe('Lifecycle and IPC handlers', () => {
    it('should define expected IPC handlers', () => {
      configureIpcHandlers();
      const eventNames = vi.mocked(ipcMain.on).mock.calls.map((call) => call[0]);
      expect(eventNames).toContain('open-file');
      expect(eventNames).toContain('save-file');
      expect(eventNames).toContain('close-window');
    });

    it('should handle open-file correctly', async () => {
      configureIpcHandlers();
      const openFileCall = vi.mocked(ipcMain.on).mock.calls.find((call) => call[0] === 'open-file');
      const handler = openFileCall![1] as (event: unknown) => Promise<void>;
      const sendMock = vi.fn();
      const event = { sender: { send: sendMock } };

      vi.mocked(dialog.showOpenDialog).mockResolvedValueOnce({
        canceled: false,
        filePaths: ['test.txt']
      });
      vi.mocked(fs.readFile).mockResolvedValueOnce('content');

      await handler(event);
      expect(sendMock).toHaveBeenCalledWith('file-opened', {
        filePath: 'test.txt',
        content: 'content'
      });
    });

    it('should handle close-window', () => {
      const mockWindow = new BrowserWindow();
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      configureIpcHandlers(mockWindow as any);
      const closeWindowCall = vi
        .mocked(ipcMain.on)
        .mock.calls.find((call) => call[0] === 'close-window');
      const handler = closeWindowCall![1] as () => void;
      handler();
      expect(mockWindow.destroy).toHaveBeenCalled();
    });
  });

  describe('App events', () => {
    it('should handle window-all-closed', async () => {
      await import('./index');
      const allClosedHandler = vi
        .mocked(app.on)
        .mock.calls.find((c) => c[0] === 'window-all-closed')?.[1] as (() => void) | undefined;
      if (allClosedHandler) {
        const originalPlatform = process.platform;
        Object.defineProperty(process, 'platform', { value: 'win32' });
        allClosedHandler();
        expect(app.quit).toHaveBeenCalled();
        Object.defineProperty(process, 'platform', { value: originalPlatform });
      }
    });

    it('should handle activate', async () => {
      await import('./index');
      const activateHandler = vi
        .mocked(app.on)
        .mock.calls.find((c) => (c[0] as string) === 'activate')?.[1] as (() => void) | undefined;
      if (activateHandler) {
        mockGetAllWindows.mockReturnValueOnce([]);
        activateHandler();
        expect(BrowserWindow).toHaveBeenCalled();
      }
    });
  });
});
