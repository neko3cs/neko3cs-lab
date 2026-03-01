import React from 'react';
import { StatusBar } from '../molecules/StatusBar';

interface AppFooterProps {
  filePath: string;
  isDirty: boolean;
}

export const AppFooter: React.FC<AppFooterProps> = ({ filePath, isDirty }) => {
  return (
    <footer className="flex items-center px-4 h-8 bg-gray-100 border-t border-gray-200 text-xs text-gray-500">
      <StatusBar filePath={filePath} isDirty={isDirty} />
    </footer>
  );
};
