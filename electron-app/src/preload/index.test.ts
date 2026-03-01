import { vi, describe, it, expect, beforeEach } from 'vitest';

vi.mock('electron', () => {
  const mockContextBridge = {
    exposeInMainWorld: vi.fn()
  };
  const mockIpcRenderer = {
    send: vi.fn(),
    on: vi.fn(),
    invoke: vi.fn()
  };
  return {
    contextBridge: mockContextBridge,
    ipcRenderer: mockIpcRenderer,
    default: {
      contextBridge: mockContextBridge,
      ipcRenderer: mockIpcRenderer
    }
  };
});

vi.mock('@electron-toolkit/preload', () => ({
  electronAPI: {
    process: {
      versions: {
        node: '1.0.0',
        chrome: '1.0.0',
        electron: '1.0.0'
      }
    }
  }
}));

import electron from 'electron';
const { contextBridge, ipcRenderer } = electron;

describe('Preload script', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    // @ts-ignore - process.contextIsolated is a read-only property in Electron but we need to mock it for testing
    process.contextIsolated = true;
  });

  it('should expose the api to the main world when contextIsolated is true', async () => {
    vi.resetModules();
    await import('./index');
    expect(contextBridge.exposeInMainWorld).toHaveBeenCalledWith('api', expect.any(Object));
  });

  it('should call ipcRenderer methods from exposed api', async () => {
    vi.resetModules();
    await import('./index');
    const apiCall = vi
      .mocked(contextBridge.exposeInMainWorld)
      .mock.calls.find((call) => call[0] === 'api');
    const exposedApi = apiCall![1] as Record<string, (...args: unknown[]) => unknown>;

    exposedApi.ping();
    expect(ipcRenderer.send).toHaveBeenCalledWith('ping');

    exposedApi.sayHello('User');
    expect(ipcRenderer.send).toHaveBeenCalledWith('say-hello', 'User');

    exposedApi.printUser({ name: 'Alice', age: 25 });
    expect(ipcRenderer.send).toHaveBeenCalledWith('print-user', { name: 'Alice', age: 25 });

    exposedApi.openFile();
    expect(ipcRenderer.send).toHaveBeenCalledWith('open-file');

    exposedApi.saveFile('data', 'path.txt');
    expect(ipcRenderer.send).toHaveBeenCalledWith('save-file', {
      content: 'data',
      filePath: 'path.txt'
    });

    exposedApi.closeWindow();
    expect(ipcRenderer.send).toHaveBeenCalledWith('close-window');
  });

  it('should handle onFileOpened callback', async () => {
    vi.resetModules();
    await import('./index');
    const apiCall = vi
      .mocked(contextBridge.exposeInMainWorld)
      .mock.calls.find((call) => call[0] === 'api');
    const exposedApi = apiCall![1] as Record<string, (...args: unknown[]) => unknown>;

    const callback = vi.fn();
    const handler = exposedApi.onFileOpened as (cb: (...args: unknown[]) => void) => void;
    handler(callback);

    expect(ipcRenderer.on).toHaveBeenCalledWith('file-opened', expect.any(Function));

    // Trigger the callback
    const onCall = vi.mocked(ipcRenderer.on).mock.calls.find((call) => call[0] === 'file-opened');
    const internalCallback = onCall![1] as (event: unknown, data: unknown) => void;
    internalCallback({}, { filePath: 'test.txt', content: 'test' });

    expect(callback).toHaveBeenCalledWith('test.txt', 'test');
  });

  it('should handle onFileSaved callback', async () => {
    vi.resetModules();
    await import('./index');
    const apiCall = vi
      .mocked(contextBridge.exposeInMainWorld)
      .mock.calls.find((call) => call[0] === 'api');
    const exposedApi = apiCall![1] as Record<string, (...args: unknown[]) => unknown>;

    const callback = vi.fn();
    const handler = exposedApi.onFileSaved as (cb: (...args: unknown[]) => void) => void;
    handler(callback);

    expect(ipcRenderer.on).toHaveBeenCalledWith('file-saved', expect.any(Function));

    const onCall = vi.mocked(ipcRenderer.on).mock.calls.find((call) => call[0] === 'file-saved');
    const internalCallback = onCall![1] as (event: unknown, data: unknown) => void;
    internalCallback({}, 'saved.txt');

    expect(callback).toHaveBeenCalledWith('saved.txt');
  });

  it('should handle onCheckUnsavedChanges callback', async () => {
    vi.resetModules();
    await import('./index');
    const apiCall = vi
      .mocked(contextBridge.exposeInMainWorld)
      .mock.calls.find((call) => call[0] === 'api');
    const exposedApi = apiCall![1] as Record<string, (...args: unknown[]) => unknown>;

    const callback = vi.fn();
    const handler = exposedApi.onCheckUnsavedChanges as (cb: (...args: unknown[]) => void) => void;
    handler(callback);

    expect(ipcRenderer.on).toHaveBeenCalledWith('check-for-unsaved-changes', expect.any(Function));

    const onCall = vi
      .mocked(ipcRenderer.on)
      .mock.calls.find((call) => call[0] === 'check-for-unsaved-changes');
    const internalCallback = onCall![1] as (event: unknown) => void;
    internalCallback({});

    expect(callback).toHaveBeenCalled();
  });

  it('should expose the api to window when contextIsolated is false', async () => {
    // @ts-ignore - process.contextIsolated is a read-only property in Electron but we need to mock it for testing
    process.contextIsolated = false;
    vi.resetModules();
    await import('./index');

    // @ts-ignore - window.api is defined when contextIsolated is false
    expect(window.api).toBeDefined();
    // @ts-ignore - window.api.ping is defined when contextIsolated is false
    expect(window.api.ping).toBeDefined();
  });
});
