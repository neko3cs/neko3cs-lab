import { vi } from 'vitest'

vi.mock('electron', () => {
  class MockBrowserWindow {
    on = vi.fn()
    show = vi.fn()
    loadURL = vi.fn()
    loadFile = vi.fn()
    webContents = {
      setWindowOpenHandler: vi.fn(),
    }
    static getAllWindows = vi.fn(() => [])
  }

  return {
    app: {
      whenReady: vi.fn().mockResolvedValue(true),
      on: vi.fn(),
      quit: vi.fn(),
    },
    shell: {
      openExternal: vi.fn(),
    },
    BrowserWindow: MockBrowserWindow,
    ipcMain: {
      on: vi.fn(),
      handle: vi.fn(),
      eventNames: vi.fn(() => ['open-file']),
    },
    dialog: {
      showOpenDialog: vi.fn().mockResolvedValue({ canceled: false, filePaths: ['test.txt'] }),
      showSaveDialog: vi.fn(),
    },
  }
})

vi.mock('@electron-toolkit/utils', () => ({
  electronApp: {
    setAppUserModelId: vi.fn(),
  },
  optimizer: {
    watchWindowShortcuts: vi.fn(),
  },
  is: {
    dev: true,
  },
}))
