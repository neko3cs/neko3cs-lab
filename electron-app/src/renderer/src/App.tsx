import { useState, useEffect, useRef } from 'react'
import { MenuBar } from './components/molecules/MenuBar'
import { TextArea } from './components/atoms/TextArea'
import { StatusBar } from './components/molecules/StatusBar'

function App(): React.JSX.Element {
  const [fileContent, setFileContent] = useState<string>('')
  const [filePath, setFilePath] = useState<string>('')
  const [isDirty, setIsDirty] = useState<boolean>(false)
  const isDirtyRef = useRef<boolean>(false)

  // Keep ref in sync with state
  useEffect(() => {
    isDirtyRef.current = isDirty
  }, [isDirty])

  useEffect(() => {
    window.api.onFileOpened((newFilePath, content) => {
      setFilePath(newFilePath)
      setFileContent(content)
      setIsDirty(false)
    })

    window.api.onFileSaved((savedPath) => {
      setFilePath(savedPath)
      setIsDirty(false)
      alert(`File saved to: ${savedPath}`)
    })

    window.api.onCheckUnsavedChanges(() => {
      if (isDirtyRef.current) {
        const choice = confirm('You have unsaved changes. Are you sure you want to quit?')
        if (choice) {
          window.api.closeWindow()
        }
      } else {
        window.api.closeWindow()
      }
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
    setIsDirty(false)
  }

  const handleContentChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => {
    setFileContent(e.target.value)
    setIsDirty(true)
  }

  return (
    <div className="flex flex-col h-screen bg-gray-50 text-gray-900">
      {/* Menu Bar Placeholder */}
      <header className="flex items-center px-4 h-12 bg-white border-b border-gray-200 shadow-sm">
        <MenuBar onNew={handleNewFile} onOpen={handleOpenFile} onSave={handleSaveFile} />
      </header>

      {/* Editor Area Placeholder */}
      <main className="flex-1 overflow-hidden p-4">
        <div className="h-full w-full bg-white rounded-lg border border-gray-200 shadow-inner p-4">
          <TextArea
            placeholder="Start typing your notes here..."
            value={fileContent}
            onChange={handleContentChange}
          />
        </div>
      </main>

      {/* Status Bar Placeholder */}
      <footer className="flex items-center px-4 h-8 bg-gray-100 border-t border-gray-200 text-xs text-gray-500">
        <StatusBar filePath={filePath} isDirty={isDirty} />
      </footer>
    </div>
  )
}

export default App
