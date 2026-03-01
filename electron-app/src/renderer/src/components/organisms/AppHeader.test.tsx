import { render, screen } from '@testing-library/react';
import { AppHeader } from './AppHeader';
import { describe, it, expect } from 'vitest';

describe('AppHeader Organism', () => {
  it('renders correctly', () => {
    render(<AppHeader onNew={() => {}} onOpen={() => {}} onSave={() => {}} />);
    expect(screen.getByRole('banner')).toBeInTheDocument();
    expect(screen.getByText('New')).toBeInTheDocument();
  });
});
