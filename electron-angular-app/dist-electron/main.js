"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const electron_1 = require("electron");
const path = require("path");
function createWindow() {
    const win = new electron_1.BrowserWindow({
        width: 1200,
        height: 800,
        webPreferences: {
            preload: path.join(__dirname, "preload.js"),
            contextIsolation: true,
        },
    });
    const devUrl = "http://localhost:4200";
    win.loadURL(devUrl).catch(() => {
        win.loadFile(path.join(__dirname, "../dist/electron-angular-app/browser/index.html"));
    });
}
function configureIPCHandlers() {
    electron_1.ipcMain.handle("ping", () => console.log("pong"));
}
electron_1.app.whenReady().then(() => {
    configureIPCHandlers();
    createWindow();
});
electron_1.app.on("window-all-closed", () => {
    if (process.platform !== "darwin") {
        electron_1.app.quit();
    }
});
