import { vi } from 'vitest'

export const app = {
  whenReady: vi.fn().mockResolvedValue(true),
  on: vi.fn(),
  quit: vi.fn(),
}

export const shell = {
  openExternal: vi.fn(),
}

export const BrowserWindow = vi.fn().mockImplementation(() => ({
  on: vi.fn(),
  show: vi.fn(),
  loadURL: vi.fn(),
  loadFile: vi.fn(),
  webContents: {
    setWindowOpenHandler: vi.fn(),
  },
}))

export const ipcMain = {
  on: vi.fn(),
  handle: vi.fn(),
  eventNames: vi.fn(() => []),
}

export const dialog = {
  showOpenDialog: vi.fn().mockResolvedValue({ canceled: false, filePaths: ['test.txt'] }),
  showSaveDialog: vi.fn(),
}
