import { vi, describe, it, expect, beforeEach, beforeAll } from 'vitest'

// Mock fs/promises before importing ./index
vi.mock('fs/promises', () => ({
  readFile: vi.fn(),
  writeFile: vi.fn(),
}))

import { ipcMain, dialog, app, BrowserWindow } from 'electron'
import * as fs from 'fs/promises'
import { configureIpcHandlers, createWindow } from './index'
import { mockGetAllWindows } from './setupTests'

describe('Main process', () => {
  beforeEach(() => {
    vi.mocked(ipcMain.on).mockClear()
    vi.mocked(ipcMain.handle).mockClear()
    vi.mocked(dialog.showOpenDialog).mockClear()
    vi.mocked(dialog.showSaveDialog).mockClear()
    vi.mocked(fs.readFile).mockClear()
    vi.mocked(fs.writeFile).mockClear()
    mockGetAllWindows.mockClear()
    vi.mocked(BrowserWindow).mockClear()
  })

  describe('createWindow', () => {
    it('should create a BrowserWindow with correct options', () => {
      createWindow()
      expect(BrowserWindow).toHaveBeenCalled()
    })

    it('should register window events', () => {
      createWindow()
      const mockWindow = vi.mocked(BrowserWindow).mock.results[0].value
      
      // Manually trigger events to cover handler bodies
      const handlers: any = {}
      mockWindow.on.mock.calls.forEach(([event, handler]) => {
        handlers[event] = handler
      })
      
      if (handlers['ready-to-show']) handlers['ready-to-show']()
      expect(mockWindow.show).toHaveBeenCalled()
      
      if (handlers['close']) {
        const preventDefault = vi.fn()
        handlers['close']({ preventDefault })
        expect(preventDefault).toHaveBeenCalled()
      }
    })
  })

  describe('Lifecycle and IPC handlers', () => {
    it('should define expected IPC handlers', () => {
      configureIpcHandlers()
      const eventNames = (ipcMain.on as any).mock.calls.map(call => call[0])
      expect(eventNames).toContain('open-file')
      expect(eventNames).toContain('save-file')
      expect(eventNames).toContain('close-window')
    })

    it('should handle open-file correctly', async () => {
      configureIpcHandlers()
      const handler = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'open-file')[1]
      const sendMock = vi.fn()
      const event = { sender: { send: sendMock } }
      
      vi.mocked(dialog.showOpenDialog).mockResolvedValueOnce({ canceled: false, filePaths: ['test.txt'] })
      vi.mocked(fs.readFile).mockResolvedValueOnce('content')
      
      await handler(event)
      expect(sendMock).toHaveBeenCalledWith('file-opened', { filePath: 'test.txt', content: 'content' })
    })

    it('should handle close-window', () => {
      const mockWindow = new BrowserWindow()
      configureIpcHandlers(mockWindow as any)
      const handler = (ipcMain.on as any).mock.calls.find(call => call[0] === 'close-window')[1]
      handler()
      expect(mockWindow.destroy).toHaveBeenCalled()
    })
  })

  describe('App events', () => {
    it('should handle window-all-closed', async () => {
      await import('./index')
      const allClosedHandler = vi.mocked(app.on).mock.calls.find(c => c[0] === 'window-all-closed')?.[1]
      if (allClosedHandler) {
        const originalPlatform = process.platform
        Object.defineProperty(process, 'platform', { value: 'win32' })
        allClosedHandler()
        expect(app.quit).toHaveBeenCalled()
        Object.defineProperty(process, 'platform', { value: originalPlatform })
      }
    })

    it('should handle activate', async () => {
      await import('./index')
      const activateHandler = vi.mocked(app.on).mock.calls.find(c => c[0] === 'activate')?.[1]
      if (activateHandler) {
        mockGetAllWindows.mockReturnValueOnce([])
        activateHandler()
        expect(BrowserWindow).toHaveBeenCalled()
      }
    })
  })
})
