import { render, screen, fireEvent } from '@testing-library/react'
import { MenuBar } from './MenuBar'
import { vi, describe, it, expect } from 'vitest'

describe('MenuBar Molecule', () => {
  it('renders all buttons', () => {
    render(<MenuBar onNew={() => {}} onOpen={() => {}} onSave={() => {}} />)
    expect(screen.getByText('New')).toBeInTheDocument()
    expect(screen.getByText('Open')).toBeInTheDocument()
    expect(screen.getByText('Save')).toBeInTheDocument()
  })

  it('calls handlers when buttons are clicked', () => {
    const onNew = vi.fn()
    const onOpen = vi.fn()
    const onSave = vi.fn()
    render(<MenuBar onNew={onNew} onOpen={onOpen} onSave={onSave} />)
    
    fireEvent.click(screen.getByText('New'))
    expect(onNew).toHaveBeenCalledTimes(1)
    
    fireEvent.click(screen.getByText('Open'))
    expect(onOpen).toHaveBeenCalledTimes(1)
    
    fireEvent.click(screen.getByText('Save'))
    expect(onSave).toHaveBeenCalledTimes(1)
  })
})
