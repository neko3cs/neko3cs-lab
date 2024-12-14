import ReactDOM from 'react-dom/client'
import MicroFrontrendComponent from "./MicroFrontrendComponent";

const roots: Record<string, ReactDOM.Root> = {};

export const mount = (containerId: string) => {
  const container = document.getElementById(containerId);
  if (container) {
    const root = ReactDOM.createRoot(container);
    root.render(<MicroFrontrendComponent />);
    roots[containerId] = root;
  }
};

export const unmount = (containerId: string) => {
  const root = roots[containerId];
  if (root) {
    root.unmount();
    delete roots[containerId];
  }
};
