const { app, BrowserWindow } = require('electron')
const url = require("url");
const path = require("path");
 
let mainWindow
 
function createWindow() {
    mainWindow = new BrowserWindow({
        width: 800,
        height: 600,
        webPreferences: {
            nodeIntegration: true,
            webSecurity: false
        }
    })
 
    mainWindow.loadURL(url.format({
        pathname: path.join('dist/client-app/index.html'),
        protocol: 'file:',
        slashes: true
      }));
    // Open the DevTools.
    mainWindow.webContents.openDevTools()
 
    mainWindow.on('closed', function() {
        mainWindow = null
    })
}
 
app.on('ready', createWindow)
 
app.on('window-all-closed', function() {
    if (process.platform !== 'darwin') app.quit()
})
 
app.on('activate', function() {
    if (mainWindow === null) createWindow()
})