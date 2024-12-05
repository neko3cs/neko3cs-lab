import React from 'react';
import { useMicroFrontend } from './hooks/useMicroFrontend';

const App = () => {
  const FeatureA = useMicroFrontend('http://localhost:5050/FeatureA.js', 'FeatureA');
  const FeatureB = useMicroFrontend('http://localhost:5050/FeatureB.js', 'FeatureB');

  return (
    <div>
      <h1>ホストアプリケーション</h1>
      <div>
        {FeatureA ? <FeatureA /> : <p>Loading FeatureA...</p>}
        {FeatureB ? <FeatureB /> : <p>Loading FeatureB...</p>}
      </div>
    </div>
  );
};

export default App;
