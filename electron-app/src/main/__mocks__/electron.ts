import { vi } from 'vitest';

export const app: {
  whenReady: () => Promise<boolean>;
  on: (event: string, listener: (...args: unknown[]) => void) => void;
  quit: () => void;
} = {
  whenReady: vi.fn().mockResolvedValue(true),
  on: vi.fn(),
  quit: vi.fn()
};

export const shell: {
  openExternal: (url: string) => Promise<void>;
} = {
  openExternal: vi.fn()
};

// BrowserWindow needs to be a constructor mock
export const BrowserWindow: unknown = vi.fn().mockImplementation(() => ({
  on: vi.fn(),
  show: vi.fn(),
  loadURL: vi.fn(),
  loadFile: vi.fn(),
  webContents: {
    setWindowOpenHandler: vi.fn()
  }
}));

export const ipcMain: {
  on: (channel: string, listener: (...args: unknown[]) => void) => void;
  handle: (channel: string, listener: (...args: unknown[]) => Promise<unknown> | unknown) => void;
  eventNames: () => string[];
} = {
  on: vi.fn(),
  handle: vi.fn(),
  eventNames: vi.fn(() => [])
};

export const dialog: {
  showOpenDialog: (options: unknown) => Promise<{ canceled: boolean; filePaths: string[] }>;
  showSaveDialog: (options: unknown) => Promise<{ canceled: boolean; filePath: string }>;
} = {
  showOpenDialog: vi.fn().mockResolvedValue({ canceled: false, filePaths: ['test.txt'] }),
  showSaveDialog: vi.fn()
};
