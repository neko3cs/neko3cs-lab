import React from 'react'

interface StatusBarProps {
  filePath: string
  isDirty: boolean
}

export const StatusBar: React.FC<StatusBarProps> = ({ filePath, isDirty }) => {
  return (
    <div className="flex-1 truncate">
      {filePath ? `File: ${filePath}` : 'New File'} {isDirty && '(Unsaved)'}
    </div>
  )
}
