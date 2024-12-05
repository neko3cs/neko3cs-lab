import { useState, useEffect } from 'react';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export const useMicroFrontend = <T = any>(url: string, moduleName: string) => {
  const [Component, setComponent] = useState<React.ComponentType<T> | null>(null);

  useEffect(() => {
    const loadComponent = async () => {
      try {
        const module = await import(/* @vite-ignore */ url);
        setComponent(() => module[moduleName]);
      } catch (err) {
        console.error(`Failed to load ${moduleName} from ${url}`, err);
      }
    };

    loadComponent();
  }, [url, moduleName]);

  return Component;
};
