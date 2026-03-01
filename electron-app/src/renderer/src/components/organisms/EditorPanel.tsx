import React from 'react';
import { TextArea } from '../atoms/TextArea';

interface EditorPanelProps {
  value: string;
  onChange: (e: React.ChangeEvent<HTMLTextAreaElement>) => void;
  placeholder?: string;
}

export const EditorPanel: React.FC<EditorPanelProps> = ({ value, onChange, placeholder }) => {
  return (
    <div className="h-full w-full bg-white rounded-lg border border-gray-200 shadow-inner p-4">
      <TextArea placeholder={placeholder} value={value} onChange={onChange} />
    </div>
  );
};
