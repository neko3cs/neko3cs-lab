import React from 'react';
import { Text } from '@fluentui/react';

const titleStyle = {
  root: {
    marginBottom: '20px'
  }
};
const textStyle = {
  root: {
    textAlign: 'center',
    color: '#333'
  }
};

const MicroFrontrendComponent: React.FC = () => {
  return (
    <div>
      <Text
        variant='xxLarge'
        styles={titleStyle}>
        Micro Frontend
      </Text>
      <Text variant="large" styles={textStyle}>
        これはマイクロフロントエンドのコンポーネントです。
      </Text>
    </div>
  );
};

export default MicroFrontrendComponent;
