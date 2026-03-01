import { render, screen, fireEvent } from '@testing-library/react'
import { Button } from './Button'
import { vi, describe, it, expect } from 'vitest'

describe('Button Atom', () => {
  it('renders children correctly', () => {
    render(<Button>Click me</Button>)
    expect(screen.getByText('Click me')).toBeInTheDocument()
  })

  it('calls onClick when clicked', () => {
    const handleClick = vi.fn()
    render(<Button onClick={handleClick}>Click me</Button>)
    fireEvent.click(screen.getByText('Click me'))
    expect(handleClick).toHaveBeenCalledTimes(1)
  })

  it('applies custom className', () => {
    const { container } = render(<Button className="custom-class">Click me</Button>)
    expect(container.firstChild).toHaveClass('custom-class')
  })
})
