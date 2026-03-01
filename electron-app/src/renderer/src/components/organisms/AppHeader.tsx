import React from 'react'
import { MenuBar } from '../molecules/MenuBar'

interface AppHeaderProps {
  onNew: () => void
  onOpen: () => void
  onSave: () => void
}

export const AppHeader: React.FC<AppHeaderProps> = ({ onNew, onOpen, onSave }) => {
  return (
    <header className="flex items-center px-4 h-12 bg-white border-b border-gray-200 shadow-sm">
      <MenuBar onNew={onNew} onOpen={onOpen} onSave={onSave} />
    </header>
  )
}
