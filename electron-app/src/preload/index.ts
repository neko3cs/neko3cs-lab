import { contextBridge, ipcRenderer } from 'electron'
import { electronAPI } from '@electron-toolkit/preload'

// Custom APIs for renderer
const api = {
  ping: (): void => ipcRenderer.send('ping'),
  sayHello: (name?: string) => ipcRenderer.send('say-hello', name),
  printUser: (user: { name: string; age: number }) => ipcRenderer.send('print-user', user),
  openFile: (): void => ipcRenderer.send('open-file'),
  onFileOpened: (callback: (filePath: string, content: string) => void) => {
    ipcRenderer.on('file-opened', (_event, { filePath, content }) => callback(filePath, content))
  }
}

// Use `contextBridge` APIs to expose Electron APIs to
// renderer only if context isolation is enabled, otherwise
// just add to the DOM global.
if (process.contextIsolated) {
  try {
    contextBridge.exposeInMainWorld('electron', electronAPI)
    contextBridge.exposeInMainWorld('api', api)
  } catch (error) {
    console.error(error)
  }
} else {
  // @ts-ignore (define in dts)
  window.electron = electronAPI
  // @ts-ignore (define in dts)
  window.api = api
}
