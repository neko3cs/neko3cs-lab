import { useState, useEffect } from 'react';

interface MicroFrontendModule {
  mount: (containerId: string) => void;
  unmount: (containerId: string) => void;
}

export const useMicroFrontend = (url: string) => {
  const [microFrontend, setMicroFrontend] = useState<MicroFrontendModule | null>(null);
  useEffect(() => {
    let isMounted = true;
    const loadMicroFrontendComponent = async () => {
      try {
        const { mount, unmount } = await import(/* @vite-ignore */ url);
        if (isMounted) {
          setMicroFrontend({ mount: mount, unmount: unmount });
        }
      } catch (err) {
        console.error(`Failed to load MicroFrontend from ${url}`, err);
      }
    };

    loadMicroFrontendComponent();

    return () => {
      isMounted = false;
    };
  }, [url]);

  return microFrontend;
};
