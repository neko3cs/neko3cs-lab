declare global {
  interface Window {
    electronAPI: {
      ping: () => Promise<void>;
    };
  }
}

export { };
