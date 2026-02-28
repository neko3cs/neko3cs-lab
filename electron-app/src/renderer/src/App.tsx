import { useState, useEffect } from 'react'
import Versions from './components/Versions'

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
    <div className="flex flex-col h-screen bg-gray-50 text-gray-900">
      {/* Menu Bar Placeholder */}
      <header className="flex items-center px-4 h-12 bg-white border-b border-gray-200 shadow-sm">
        <div className="flex space-x-4">
          <button
            onClick={handleNewFile}
            className="text-sm font-medium text-gray-600 hover:text-blue-600 transition-colors"
          >
            New
          </button>
          <button
            onClick={handleOpenFile}
            className="text-sm font-medium text-gray-600 hover:text-blue-600 transition-colors"
          >
            Open
          </button>
          <button
            onClick={handleSaveFile}
            className="text-sm font-medium text-gray-600 hover:text-blue-600 transition-colors"
          >
            Save
          </button>
        </div>
      </header>

      {/* Editor Area Placeholder */}
      <main className="flex-1 overflow-hidden p-4">
        <div className="h-full w-full bg-white rounded-lg border border-gray-200 shadow-inner p-4">
          <textarea
            className="w-full h-full resize-none focus:outline-none text-lg leading-relaxed"
            placeholder="Start typing your notes here..."
            value={fileContent}
            onChange={(e) => setFileContent(e.target.value)}
          ></textarea>
        </div>
      </main>

      {/* Status Bar Placeholder */}
      <footer className="flex items-center px-4 h-8 bg-gray-100 border-t border-gray-200 text-xs text-gray-500">
        <div className="flex-1 truncate">{filePath ? `File: ${filePath}` : 'New File'}</div>
        <div className="flex items-center space-x-4">
          <Versions />
        </div>
      </footer>
    </div>
  )
}

export default App
