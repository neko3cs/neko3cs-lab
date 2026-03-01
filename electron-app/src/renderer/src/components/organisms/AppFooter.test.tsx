import { render, screen } from '@testing-library/react';
import { AppFooter } from './AppFooter';
import { describe, it, expect } from 'vitest';

describe('AppFooter Organism', () => {
  it('renders correctly', () => {
    render(<AppFooter filePath="test.txt" isDirty={false} />);
    expect(screen.getByRole('contentinfo')).toBeInTheDocument();
    expect(screen.getByText('File: test.txt')).toBeInTheDocument();
  });
});
