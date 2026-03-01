import React from 'react';

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  children: React.ReactNode;
}

export const Button: React.FC<ButtonProps> = ({ children, className = '', ...props }) => {
  return (
    <button
      className={`text-sm font-medium text-gray-600 hover:text-blue-600 transition-colors ${className}`}
      {...props}
    >
      {children}
    </button>
  );
};
