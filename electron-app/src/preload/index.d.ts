import { ElectronAPI } from '@electron-toolkit/preload'

declare global {
  interface Window {
    electron: ElectronAPI
    api: {
      ping: () => void
      sayHello: (name?: string) => void
      printUser: (user: { name: string; age: number }) => void
      openFile: () => void
      onFileOpened: (callback: (filePath: string, content: string) => void) => void
      saveFile: (content: string, filePath?: string) => void
      onFileSaved: (callback: (filePath: string) => void) => void
      onCheckUnsavedChanges: (callback: () => void) => void
      closeWindow: () => void
    }
  }
}
