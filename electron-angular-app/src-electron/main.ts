import { app, BrowserWindow, ipcMain } from "electron";
import * as path from "path";

function createWindow() {
  const win = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      preload: path.join(__dirname, "preload.js"),
      contextIsolation: true,
    },
  });

  const devUrl = "http://localhost:4200";

  win.loadURL(devUrl).catch(() => {
    win.loadFile(
      path.join(
        __dirname,
        "../dist/electron-angular-app/browser/index.html"
      )
    );
  });
}

function configureIPCHandlers() {
  ipcMain.handle("ping", () => console.log("pong"));
}

app.whenReady().then(() => {
  configureIPCHandlers();
  createWindow();
});

app.on("window-all-closed", () => {
  if (process.platform !== "darwin") {
    app.quit();
  }
});
