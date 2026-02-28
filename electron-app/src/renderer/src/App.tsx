import { useState, useEffect } from 'react'
import Versions from './components/Versions'
import electronLogo from './assets/electron.svg'

function App(): React.JSX.Element {
  const [fileContent, setFileContent] = useState<string>('')
  const [filePath, setFilePath] = useState<string>('')

  useEffect(() => {
    window.api.onFileOpened((newFilePath, content) => {
      setFilePath(newFilePath)
      setFileContent(content)
    })

    window.api.onFileSaved((savedPath) => {
      setFilePath(savedPath)
      alert(`File saved to: ${savedPath}`)
    })
  }, [])

  const handleOpenFile = () => {
    window.api.openFile()
  }

  const handleSaveFile = () => {
    window.api.saveFile(fileContent, filePath)
  }

  const handleNewFile = () => {
    setFilePath('')
    setFileContent('')
  }

  return (
    <>
      <img alt="logo" className="logo" src={electronLogo} />
      <div className="creator">Powered by electron-vite</div>
      <div className="text">
        Build an Electron app with <span className="react">React</span>
        &nbsp;and <span className="ts">TypeScript</span>
      </div>
      <p className="tip">
        Please try pressing <code>F12</code> to open the devTool
      </p>
      <div className="actions">
        <div className="action">
          <a href="https://electron-vite.org/" target="_blank" rel="noreferrer">
            Documentation
          </a>
        </div>
        <div className="action">
          <a target="_blank" rel="noreferrer" onClick={handleNewFile}>
            New File
          </a>
        </div>
        <div className="action">
          <a target="_blank" rel="noreferrer" onClick={handleOpenFile}>
            Open File
          </a>
        </div>
        <div className="action">
          <a target="_blank" rel="noreferrer" onClick={handleSaveFile}>
            Save File
          </a>
        </div>
      </div>
      {filePath ? <p>File: {filePath}</p> : <p>New File</p>}
      {fileContent !== undefined && (
        <textarea
          className="w-4/5 h-[200px] mt-5 p-2 border border-gray-300 rounded shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
          value={fileContent}
          onChange={(e) => setFileContent(e.target.value)}
        ></textarea>
      )}
      <Versions></Versions>
    </>
  )
}

export default App
