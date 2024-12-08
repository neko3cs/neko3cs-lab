import React from 'react';
import { useMicroFrontend } from './hooks/useMicroFrontend';

const App = () => {
  const MicroFrontrendComponent = useMicroFrontend(
    'http://localhost:5050/MicroFrontrendComponent.js',
    'MicroFrontrendComponent'
  );

  return (
    <div>
      <h1>ホストアプリケーション</h1>
      <div>
        {MicroFrontrendComponent ? <MicroFrontrendComponent /> : <p>Loading MicroFrontrendComponent...</p>}
      </div>
    </div>
  );
};

export default App;
