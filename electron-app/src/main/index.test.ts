import { vi, describe, it, expect, beforeEach, beforeAll } from 'vitest'

// Mock fs/promises before importing ./index
vi.mock('fs/promises', () => ({
  readFile: vi.fn(),
  writeFile: vi.fn(),
}))

import { ipcMain, dialog } from 'electron'
import * as fs from 'fs/promises'

describe('ipcMain handlers', () => {
  beforeAll(async () => {
    await import('./index')
    // Wait for app.whenReady().then(...) to execute
    await new Promise(resolve => setTimeout(resolve, 10))
  })

  beforeEach(() => {
    // We don't want to clear all mocks because that would clear the ipcMain.on registrations
    // Instead, we clear the call history for the ones we want to track per-test
    vi.mocked(fs.readFile).mockClear()
    vi.mocked(fs.writeFile).mockClear()
    vi.mocked(dialog.showOpenDialog).mockClear()
    vi.mocked(dialog.showSaveDialog).mockClear()
  })

  describe('open-file handler', () => {
    it('should define a handler for "open-file"', () => {
      const openFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'open-file')
      expect(openFileCall).toBeDefined()
    })

    it('should call dialog.showOpenDialog when "open-file" is triggered', async () => {
      const openFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'open-file')
      const handler = openFileCall[1]
      const event = { sender: { send: vi.fn() } }
      
      vi.mocked(dialog.showOpenDialog).mockResolvedValueOnce({ canceled: true, filePaths: [] })
      
      await handler(event)
      expect(dialog.showOpenDialog).toHaveBeenCalled()
    })

    it('should send the file content to the renderer when a file is selected', async () => {
      const openFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'open-file')
      const handler = openFileCall[1]
      const sendMock = vi.fn()
      const event = { sender: { send: sendMock } }
      
      const filePath = 'test.txt'
      const content = 'Hello World\n'
      vi.mocked(dialog.showOpenDialog).mockResolvedValueOnce({ canceled: false, filePaths: [filePath] })
      vi.mocked(fs.readFile).mockResolvedValueOnce(content)
      
      await handler(event)
      
      expect(fs.readFile).toHaveBeenCalledWith(filePath, { encoding: 'utf8' })
      expect(sendMock).toHaveBeenCalledWith('file-opened', { filePath, content })
    })

    it('should send an error message when file reading fails', async () => {
      const openFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'open-file')
      const handler = openFileCall[1]
      const sendMock = vi.fn()
      const event = { sender: { send: sendMock } }
      
      const filePath = 'error.txt'
      vi.mocked(dialog.showOpenDialog).mockResolvedValueOnce({ canceled: false, filePaths: [filePath] })
      vi.mocked(fs.readFile).mockRejectedValueOnce(new Error('Read failed'))
      
      await handler(event)
      
      expect(sendMock).toHaveBeenCalledWith('file-open-error', 'Read failed')
    })
  })

  describe('save-file handler', () => {
    it('should define a handler for "save-file"', () => {
      const saveFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'save-file')
      expect(saveFileCall).toBeDefined()
    })

    it('should call fs.writeFile when "save-file" is triggered with a filePath', async () => {
      const saveFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'save-file')
      const handler = saveFileCall[1]
      const sendMock = vi.fn()
      const event = { sender: { send: sendMock } }
      
      const filePath = 'save-test.txt'
      const content = 'test content'
      vi.mocked(fs.writeFile).mockResolvedValueOnce(undefined as any)
      
      await handler(event, { content, filePath })
      
      expect(fs.writeFile).toHaveBeenCalledWith(filePath, content, { encoding: 'utf8' })
      expect(sendMock).toHaveBeenCalledWith('file-saved', filePath)
    })

    it('should show save dialog when no filePath is provided', async () => {
      const saveFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'save-file')
      const handler = saveFileCall[1]
      const event = { sender: { send: vi.fn() } }
      
      vi.mocked(dialog.showSaveDialog).mockResolvedValueOnce({ canceled: true, filePath: '' })
      
      await handler(event, { content: 'new content', filePath: '' })
      
      expect(dialog.showSaveDialog).toHaveBeenCalled()
    })

    it('should send an error message when file saving fails', async () => {
      const saveFileCall = (ipcMain.on as any).mock.calls.find((call) => call[0] === 'save-file')
      const handler = saveFileCall[1]
      const sendMock = vi.fn()
      const event = { sender: { send: sendMock } }
      
      const filePath = 'readonly.txt'
      vi.mocked(fs.writeFile).mockRejectedValueOnce(new Error('Write failed'))
      
      await handler(event, { content: 'content', filePath })
      
      expect(sendMock).toHaveBeenCalledWith('file-save-error', 'Write failed')
    })
  })
})
