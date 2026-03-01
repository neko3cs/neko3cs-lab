import { vi } from 'vitest'

export const mockGetAllWindows = vi.fn(() => [])

class MockBrowserWindow {
  on = vi.fn()
  show = vi.fn()
  loadURL = vi.fn()
  loadFile = vi.fn()
  destroy = vi.fn()
  webContents = {
    setWindowOpenHandler: vi.fn(),
    send: vi.fn(),
  }
  static getAllWindows = mockGetAllWindows
}

// MUST use a regular function, not an arrow function, to be used as a constructor
const BrowserWindowMock = vi.fn().mockImplementation(function() {
  return new MockBrowserWindow()
})
// @ts-ignore
BrowserWindowMock.getAllWindows = mockGetAllWindows

vi.mock('electron', () => {
  return {
    app: {
      whenReady: vi.fn().mockResolvedValue(true),
      on: vi.fn(),
      quit: vi.fn(),
    },
    shell: {
      openExternal: vi.fn(),
    },
    BrowserWindow: BrowserWindowMock,
    ipcMain: {
      on: vi.fn(),
      handle: vi.fn(),
      eventNames: vi.fn(() => []),
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
