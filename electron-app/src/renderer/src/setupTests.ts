import '@testing-library/jest-dom'
import { vi } from 'vitest'

// Mock window.api
const mockApi = {
  onFileOpened: vi.fn(),
  onFileSaved: vi.fn(),
  openFile: vi.fn(),
  saveFile: vi.fn(),
  onCheckUnsavedChanges: vi.fn(),
  closeWindow: vi.fn(),
}

// Mock window.electron
const mockElectron = {
  process: {
    versions: {
      node: '1.0.0',
      chrome: '1.0.0',
      electron: '1.0.0',
    },
  },
}

// @ts-ignore
window.api = mockApi
// @ts-ignore
window.electron = mockElectron
