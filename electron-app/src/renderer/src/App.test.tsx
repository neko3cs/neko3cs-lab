// @vitest-environment happy-dom
import { render, screen } from '@testing-library/react'
import App from './App'
import { vi, expect, it, describe, beforeEach } from 'vitest'

describe('App Component', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('renders the menu bar with New, Open, and Save buttons', () => {
    render(<App />)
    expect(screen.getByText('New')).toBeInTheDocument()
    expect(screen.getByText('Open')).toBeInTheDocument()
    expect(screen.getByText('Save')).toBeInTheDocument()
  })

  it('renders the editor textarea', () => {
    render(<App />)
    expect(screen.getByPlaceholderText('Start typing your notes here...')).toBeInTheDocument()
  })

  it('renders the status bar', () => {
    render(<App />)
    expect(screen.getByText('New File')).toBeInTheDocument()
  })
})
