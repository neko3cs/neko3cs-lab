import { vi, describe, it, expect } from 'vitest'

// Mock fs/promises before importing ./index
vi.mock('fs/promises', () => ({
  readFile: vi.fn().mockResolvedValue('Hello World\n'),
  writeFile: vi.fn().mockResolvedValue(undefined),
}))

import { ipcMain, dialog } from 'electron'
import * as fs from 'fs/promises'
import './index'

describe('ipcMain handlers', () => {
  it('should define a handler for "open-file"', () => {
    expect(ipcMain.on).toHaveBeenCalledWith('open-file', expect.any(Function))
  })

  it('should call dialog.showOpenDialog when "open-file" is triggered', async () => {
    const openFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'open-file')
    const handler = openFileCall[1]
    const event = { sender: { send: vi.fn() } }
    await handler(event)
    expect(dialog.showOpenDialog).toHaveBeenCalled()
  })

  it('should send the file content to the renderer when a file is selected', async () => {
    const openFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'open-file')
    const handler = openFileCall[1]
    const sendMock = vi.fn()
    const event = { sender: { send: sendMock } }
    await handler(event)
    expect(fs.readFile).toHaveBeenCalledWith('test.txt', { encoding: 'utf8' })
    expect(sendMock).toHaveBeenCalledWith('file-opened', {
      filePath: 'test.txt',
      content: 'Hello World\n'
    })
  })

  it('should define a handler for "save-file"', () => {
    expect(ipcMain.on).toHaveBeenCalledWith('save-file', expect.any(Function))
  })

  it('should call fs.writeFile when "save-file" is triggered with a filePath', async () => {
    const saveFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'save-file')
    const handler = saveFileCall[1]
    const event = { sender: { send: vi.fn() } }
    await handler(event, { content: 'test content', filePath: 'save-test.txt' })
    expect(fs.writeFile).toHaveBeenCalledWith('save-test.txt', 'test content', { encoding: 'utf8' })
  })
})
