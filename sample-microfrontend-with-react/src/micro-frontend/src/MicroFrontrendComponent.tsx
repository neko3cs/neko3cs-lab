import React from 'react';
import { Stack, Text } from '@fluentui/react';

const MicroFrontrendComponent: React.FC = () => {
  return (
    <Stack style={{ minWidth: 500, minHeight: 200, border: '2px solid #000000', borderRadius: 10, padding: 16 }}>
      <Text variant='xxLarge' styles={{ root: { marginBottom: '20px' } }}>
        Micro Frontend
      </Text>

      <Text variant="large" styles={{ root: { textAlign: 'center', color: '#333' } }}>
        これはマイクロフロントエンドのコンポーネントです。
      </Text>
    </Stack>
  );
};

export default MicroFrontrendComponent;
