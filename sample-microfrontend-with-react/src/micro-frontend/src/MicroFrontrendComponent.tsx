import React from 'react';
import { Text } from '@fluentui/react';

const textStyle = {
  root: {
    textAlign: 'center',
    color: '#333'
  }
};

const MicroFrontrendComponent: React.FC = () => {
  return (
    <Text variant="large" styles={textStyle}>
      これはマイクロフロントエンドのコンポーネントです。
    </Text>
  );
};

export default MicroFrontrendComponent;
