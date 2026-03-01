import { render, screen, fireEvent } from '@testing-library/react'
import { TextArea } from './TextArea'
import { describe, it, expect, vi } from 'vitest'

describe('TextArea Atom', () => {
  it('renders correctly with placeholder', () => {
    render(<TextArea placeholder="Test placeholder" />)
    expect(screen.getByPlaceholderText('Test placeholder')).toBeInTheDocument()
  })

  it('calls onChange when text changes', () => {
    const handleChange = vi.fn()
    render(<TextArea onChange={handleChange} />)
    fireEvent.change(screen.getByRole('textbox'), { target: { value: 'new text' } })
    expect(handleChange).toHaveBeenCalledTimes(1)
  })

  it('displays the correct value', () => {
    render(<TextArea value="initial value" readOnly />)
    expect(screen.getByRole('textbox')).toHaveValue('initial value')
  })
})
