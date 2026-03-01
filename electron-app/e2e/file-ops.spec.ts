import { _electron as electron } from 'playwright';
import { test, expect } from '@playwright/test';
import path from 'path';

test('file operations basic e2e', async () => {
  const electronApp = await electron.launch({
    args: [path.join(__dirname, '..', 'out/main/index.js')]
  });

  const window = await electronApp.firstWindow();

  // 1. Test New File
  await window.getByRole('button', { name: 'New' }).click();
  await expect(window.getByPlaceholder('Start typing your notes here...')).toHaveValue('');

  // 2. Test Typing
  await window.getByRole('textbox').fill('E2E test content');
  await expect(window.getByText(/Unsaved/)).toBeVisible();

  // Cleanup
  await electronApp.evaluate(({ BrowserWindow }) => {
    BrowserWindow.getAllWindows().forEach((w) => w.destroy());
  });
  await electronApp.close();
});
