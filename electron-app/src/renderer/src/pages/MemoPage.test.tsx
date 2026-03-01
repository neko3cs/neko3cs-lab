// @vitest-environment happy-dom
import { render, screen, fireEvent, waitFor, act } from '@testing-library/react';
import { MemoPage } from './MemoPage';
import { describe, it, expect, beforeEach, vi } from 'vitest';

describe('MemoPage', () => {
  beforeEach(() => {
    vi.clearAllMocks();
    // Manual mocks for happy-dom missing globals
    window.alert = vi.fn();
    window.confirm = vi.fn();
  });

  it('renders correctly', () => {
    render(<MemoPage />);
    expect(screen.getByRole('banner')).toBeInTheDocument();
    expect(screen.getByRole('textbox')).toBeInTheDocument();
    expect(screen.getByRole('contentinfo')).toBeInTheDocument();
  });

  it('updates content when typing', () => {
    render(<MemoPage />);
    const textarea = screen.getByRole('textbox');
    fireEvent.change(textarea, { target: { value: 'new content' } });
    expect(textarea).toHaveValue('new content');
    expect(screen.getByText(/Unsaved/)).toBeInTheDocument();
  });

  it('calls handleNewFile and resets state', () => {
    render(<MemoPage />);
    const textarea = screen.getByRole('textbox');
    fireEvent.change(textarea, { target: { value: 'dirty' } });

    const newButton = screen.getByText('New');
    fireEvent.click(newButton);

    expect(textarea).toHaveValue('');
    expect(screen.queryByText(/Unsaved/)).not.toBeInTheDocument();
  });

  it('calls openFile when Open button is clicked', () => {
    render(<MemoPage />);
    const openButton = screen.getByText('Open');
    fireEvent.click(openButton);
    expect(window.api.openFile).toHaveBeenCalled();
  });

  it('calls saveFile when Save button is clicked', () => {
    render(<MemoPage />);
    const textarea = screen.getByRole('textbox');
    fireEvent.change(textarea, { target: { value: 'some text' } });

    const saveButton = screen.getByText('Save');
    fireEvent.click(saveButton);
    expect(window.api.saveFile).toHaveBeenCalledWith('some text', '');
  });

  it('handles file-opened event from main process', async () => {
    render(<MemoPage />);

    const onFileOpenedCall = vi.mocked(window.api.onFileOpened).mock.calls[0];
    const callback = onFileOpenedCall[0];

    act(() => {
      callback('test.txt', 'test content');
    });

    await waitFor(() => {
      expect(screen.getByRole('textbox')).toHaveValue('test content');
      expect(screen.getByText(/test\.txt/)).toBeInTheDocument();
    });
  });

  it('handles file-saved event from main process', async () => {
    render(<MemoPage />);

    const onFileSavedCall = vi.mocked(window.api.onFileSaved).mock.calls[0];
    const callback = onFileSavedCall[0];

    const textarea = screen.getByRole('textbox');
    fireEvent.change(textarea, { target: { value: 'saved text' } });

    act(() => {
      callback('saved.txt');
    });

    await waitFor(() => {
      expect(screen.getByText(/saved\.txt/)).toBeInTheDocument();
      expect(screen.queryByText(/Unsaved/)).not.toBeInTheDocument();
      expect(window.alert).toHaveBeenCalledWith('File saved to: saved.txt');
    });
  });

  it('handles check-for-unsaved-changes when NOT dirty', () => {
    render(<MemoPage />);

    const onCheckCall = vi.mocked(window.api.onCheckUnsavedChanges).mock.calls[0];
    const callback = onCheckCall[0];

    act(() => {
      callback();
    });

    expect(window.api.closeWindow).toHaveBeenCalled();
  });

  it('handles check-for-unsaved-changes when dirty and user confirms', () => {
    vi.mocked(window.confirm).mockReturnValue(true);

    render(<MemoPage />);

    const onCheckCall = vi.mocked(window.api.onCheckUnsavedChanges).mock.calls[0];
    const callback = onCheckCall[0];

    const textarea = screen.getByRole('textbox');
    fireEvent.change(textarea, { target: { value: 'dirty text' } });

    act(() => {
      callback();
    });

    expect(window.confirm).toHaveBeenCalled();
    expect(window.api.closeWindow).toHaveBeenCalled();
  });

  it('handles check-for-unsaved-changes when dirty and user cancels', () => {
    vi.mocked(window.confirm).mockReturnValue(false);

    render(<MemoPage />);

    const onCheckCall = vi.mocked(window.api.onCheckUnsavedChanges).mock.calls[0];
    const callback = onCheckCall[0];

    const textarea = screen.getByRole('textbox');
    fireEvent.change(textarea, { target: { value: 'dirty text' } });

    act(() => {
      callback();
    });

    expect(window.confirm).toHaveBeenCalled();
    expect(window.api.closeWindow).not.toHaveBeenCalled();
  });
});
