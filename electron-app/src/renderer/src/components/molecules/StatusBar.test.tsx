import { render, screen } from '@testing-library/react'
import { StatusBar } from './StatusBar'
import { describe, it, expect } from 'vitest'

describe('StatusBar Molecule', () => {
  it('renders "New File" when filePath is empty', () => {
    render(<StatusBar filePath="" isDirty={false} />)
    expect(screen.getByText('New File')).toBeInTheDocument()
  })

  it('renders filePath when provided', () => {
    render(<StatusBar filePath="/path/to/file.txt" isDirty={false} />)
    expect(screen.getByText('File: /path/to/file.txt')).toBeInTheDocument()
  })

  it('renders "(Unsaved)" when isDirty is true', () => {
    render(<StatusBar filePath="file.txt" isDirty={true} />)
    expect(screen.getByText('File: file.txt (Unsaved)')).toBeInTheDocument()
  })
})
