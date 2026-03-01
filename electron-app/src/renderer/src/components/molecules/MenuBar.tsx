import React from 'react'
import { Button } from '../atoms/Button'

interface MenuBarProps {
  onNew: () => void
  onOpen: () => void
  onSave: () => void
}

export const MenuBar: React.FC<MenuBarProps> = ({ onNew, onOpen, onSave }) => {
  return (
    <div className="flex space-x-4">
      <Button onClick={onNew}>New</Button>
      <Button onClick={onOpen}>Open</Button>
      <Button onClick={onSave}>Save</Button>
    </div>
  )
}
