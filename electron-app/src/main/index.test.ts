import { vi, describe, it, expect } from 'vitest'
import { ipcMain, dialog } from 'electron'
import './index'

describe('ipcMain handlers', () => {
  it('should define a handler for "open-file"', () => {
    expect(ipcMain.on).toHaveBeenCalledWith('open-file', expect.any(Function))
  })

  it('should call dialog.showOpenDialog when "open-file" is triggered', async () => {
    // Find the 'open-file' handler
    const openFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'open-file')
    if (!openFileCall) {
      throw new Error('open-file handler not found')
    }
    const handler = openFileCall[1]

    const event = { sender: { send: vi.fn() } }
    await handler(event)

    expect(dialog.showOpenDialog).toHaveBeenCalledWith({
      properties: ['openFile'],
      filters: [{ name: 'Text Files', extensions: ['txt', 'md'] }]
    })
  })

  it('should send the file content to the renderer when a file is selected', async () => {
    // Find the 'open-file' handler
    const openFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'open-file')
    const handler = openFileCall[1]

    const sendMock = vi.fn()
    const event = { sender: { send: sendMock } }
    
    await handler(event)

    expect(sendMock).toHaveBeenCalledWith('file-opened', {
      filePath: 'test.txt',
      content: 'Hello World\n'
    })
  })
})
