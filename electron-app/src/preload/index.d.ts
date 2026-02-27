import { ElectronAPI } from '@electron-toolkit/preload'

declare global {
  interface Window {
    electron: ElectronAPI
    api: {
      ping: () => void
      sayHello: (name?: string) => void
      printUser: (user: { name: string; age: number }) => void
    }
  }
}
