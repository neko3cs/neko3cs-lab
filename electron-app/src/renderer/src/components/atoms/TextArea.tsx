import React from 'react';

interface TextAreaProps extends React.TextareaHTMLAttributes<HTMLTextAreaElement> {}

export const TextArea: React.FC<TextAreaProps> = ({ className = '', ...props }) => {
  return (
    <textarea
      className={`w-full h-full resize-none focus:outline-none text-lg leading-relaxed ${className}`}
      {...props}
    />
  );
};
