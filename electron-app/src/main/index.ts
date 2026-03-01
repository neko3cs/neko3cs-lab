import { app, shell, BrowserWindow, ipcMain, dialog } from 'electron'
import * as fs from 'fs/promises'
import { join } from 'path'
import { electronApp, optimizer, is } from '@electron-toolkit/utils'
import icon from '../../resources/icon.png?asset'

let mainWindow: BrowserWindow | null = null

export function createWindow(): void {
  // Create the browser window.
  mainWindow = new BrowserWindow({
    width: 900,
    height: 670,
    show: false,
    autoHideMenuBar: true,
    ...(process.platform === 'linux' ? { icon } : {}),
    webPreferences: {
      preload: join(__dirname, '../preload/index.js'),
      sandbox: false
    }
  })

  mainWindow.on('ready-to-show', () => {
    if (mainWindow) mainWindow.show()
  })

  mainWindow.on('close', (e) => {
    if (mainWindow) {
      e.preventDefault()
      mainWindow.webContents.send('check-for-unsaved-changes')
    }
  })

  mainWindow.webContents.setWindowOpenHandler((details) => {
    shell.openExternal(details.url)
    return { action: 'deny' }
  })

  // HMR for renderer base on electron-vite cli.
  // Load the remote URL for development or the local html file for production.
  if (is.dev && process.env['ELECTRON_RENDERER_URL']) {
    mainWindow.loadURL(process.env['ELECTRON_RENDERER_URL'])
  } else {
    mainWindow.loadFile(join(__dirname, '../renderer/index.html'))
  }
}

export function configureIpcHandlers(win?: BrowserWindow): void {
  // IPC test
  ipcMain.on('ping', () => console.log('pong'))

  ipcMain.on('say-hello', (_event, name = 'World') => {
    console.log(`Hello, ${name}!`)
  })

  ipcMain.on('print-user', (_event, user: { name: string; age: number }) => {
    console.log(`${user.name} age is ${user.age}!`)
  })

  ipcMain.on('open-file', async (event) => {
    try {
      const result = await dialog.showOpenDialog({
        properties: ['openFile'],
        filters: [{ name: 'Text Files', extensions: ['txt', 'md'] }]
      })

      if (!result.canceled && result.filePaths.length > 0) {
        const filePath = result.filePaths[0]
        const content = await fs.readFile(filePath, { encoding: 'utf8' })
        event.sender.send('file-opened', { filePath, content })
      }
    } catch (err) {
      console.error('Failed to open file:', err)
      const errorMessage = err instanceof Error ? err.message : String(err)
      event.sender.send('file-open-error', errorMessage)
    }
  })

  ipcMain.on('save-file', async (event, { content, filePath }) => {
    try {
      let targetPath = filePath
      if (!targetPath) {
        const result = await dialog.showSaveDialog({
          filters: [{ name: 'Text Files', extensions: ['txt', 'md'] }]
        })
        if (result.canceled) return
        targetPath = result.filePath
      }
      if (targetPath) {
        await fs.writeFile(targetPath, content, { encoding: 'utf8' })
        event.sender.send('file-saved', targetPath)
      }
    } catch (err) {
      console.error('Failed to save file:', err)
      const errorMessage = err instanceof Error ? err.message : String(err)
      event.sender.send('file-save-error', errorMessage)
    }
  })

  ipcMain.on('close-window', () => {
    if (win) {
      win.destroy()
    } else if (mainWindow) {
      mainWindow.destroy()
      mainWindow = null
    } else {
      const allWindows = BrowserWindow.getAllWindows()
      if (allWindows.length > 0) {
        allWindows[0].destroy()
      }
    }
  })
}

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.whenReady().then(() => {
  // Set app user model id for windows
  electronApp.setAppUserModelId('com.electron')

  // Default open or close DevTools by F12 in development
  // and ignore CommandOrControl + R in production.
  // see https://github.com/alex8088/electron-toolkit/tree/master/packages/utils
  app.on('browser-window-created', (_, window) => {
    optimizer.watchWindowShortcuts(window)
  })

  configureIpcHandlers()

  createWindow()

  app.on('activate', function () {
    // On macOS it's common to re-create a window in the app when the
    // dock icon is clicked and there are no other windows open.
    if (BrowserWindow.getAllWindows().length === 0) createWindow()
  })
})

// Quit when all windows are closed, except on macOS. There, it's common
// for applications and their menu bar to stay active until the user quits
// explicitly with Cmd + Q.
app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit()
  }
})

// In this file you can include the rest of your app's specific main process
// code. You can also put them in separate files and require them here.
