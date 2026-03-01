import { render, screen } from '@testing-library/react'
import { EditorPanel } from './EditorPanel'
import { describe, it, expect, vi } from 'vitest'

describe('EditorPanel Organism', () => {
  it('renders correctly with value', () => {
    render(<EditorPanel value="test content" onChange={() => {}} />)
    expect(screen.getByRole('textbox')).toHaveValue('test content')
  })
})
