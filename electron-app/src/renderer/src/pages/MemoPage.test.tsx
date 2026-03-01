// @vitest-environment happy-dom
import { render, screen } from '@testing-library/react'
import { MemoPage } from './MemoPage'
import { describe, it, expect, beforeEach, vi } from 'vitest'

describe('MemoPage', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('renders correctly', () => {
    render(<MemoPage />)
    expect(screen.getByRole('banner')).toBeInTheDocument()
    expect(screen.getByRole('textbox')).toBeInTheDocument()
    expect(screen.getByRole('contentinfo')).toBeInTheDocument()
  })
})
