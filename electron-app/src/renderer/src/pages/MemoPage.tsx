import React, { useState, useEffect, useRef } from 'react';
import { AppHeader } from '../components/organisms/AppHeader';
import { EditorPanel } from '../components/organisms/EditorPanel';
import { AppFooter } from '../components/organisms/AppFooter';

export const MemoPage: React.FC = () => {
  const [fileContent, setFileContent] = useState<string>('');
  const [filePath, setFilePath] = useState<string>('');
  const [isDirty, setIsDirty] = useState<boolean>(false);
  const isDirtyRef = useRef<boolean>(false);

  // Keep ref in sync with state
  useEffect(() => {
    isDirtyRef.current = isDirty;
  }, [isDirty]);

  useEffect(() => {
    window.api.onFileOpened((newFilePath, content) => {
      setFilePath(newFilePath);
      setFileContent(content);
      setIsDirty(false);
    });

    window.api.onFileSaved((savedPath) => {
      setFilePath(savedPath);
      setIsDirty(false);
      alert(`File saved to: ${savedPath}`);
    });

    window.api.onCheckUnsavedChanges(() => {
      if (isDirtyRef.current) {
        const choice = confirm('You have unsaved changes. Are you sure you want to quit?');
        if (choice) {
          window.api.closeWindow();
        }
      } else {
        window.api.closeWindow();
      }
    });
  }, []);

  const handleOpenFile = (): void => {
    window.api.openFile();
  };

  const handleSaveFile = (): void => {
    window.api.saveFile(fileContent, filePath);
  };

  const handleNewFile = (): void => {
    setFilePath('');
    setFileContent('');
    setIsDirty(false);
  };

  const handleContentChange = (e: React.ChangeEvent<HTMLTextAreaElement>): void => {
    setFileContent(e.target.value);
    setIsDirty(true);
  };

  return (
    <div className="flex flex-col h-screen bg-gray-50 text-gray-900">
      <AppHeader onNew={handleNewFile} onOpen={handleOpenFile} onSave={handleSaveFile} />

      <main className="flex-1 overflow-hidden p-4">
        <EditorPanel
          placeholder="Start typing your notes here..."
          value={fileContent}
          onChange={handleContentChange}
        />
      </main>

      <AppFooter filePath={filePath} isDirty={isDirty} />
    </div>
  );
};
