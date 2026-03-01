import { _electron as electron } from 'playwright'
import { test, expect } from '@playwright/test'
import path from 'path'

test('launch app and verify UI elements', async () => {
  const electronApp = await electron.launch({
    args: [path.join(__dirname, '..', 'out/main/index.js')],
  })

  const window = await electronApp.firstWindow()

  // Verify Menu Bar buttons
  await expect(window.getByRole('button', { name: 'New' })).toBeVisible()
  await expect(window.getByRole('button', { name: 'Open' })).toBeVisible()
  await expect(window.getByRole('button', { name: 'Save' })).toBeVisible()

  // Verify Editor
  await expect(window.getByPlaceholder('Start typing your notes here...')).toBeVisible()

  // Verify Status Bar
  await expect(window.getByText('New File', { exact: false })).toBeVisible()

  // Force close all windows from the main process
  await electronApp.evaluate(({ BrowserWindow }) => {
    BrowserWindow.getAllWindows().forEach(w => w.destroy())
  })

  await electronApp.close()
})
