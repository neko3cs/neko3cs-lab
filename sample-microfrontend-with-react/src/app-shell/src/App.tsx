import React, { useEffect } from 'react';
import styled from 'styled-components';
import { DefaultButton, IStackStyles, Spinner, SpinnerSize, Stack, Text } from '@fluentui/react';
import { useMicroFrontend } from './hooks/useMicroFrontend';

const Layout = styled.div`
  display: grid;
  grid-template-rows: 60px 1fr;
  grid-template-columns: 240px 1fr;
  height: 100vh;
  grid-template-areas:
    'header header'
    'sidebar content';
`;
const Header = styled.header`
  grid-area: header;
  background-color: #0078d4;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0 20px;
`;
const Sidebar = styled.aside`
  grid-area: sidebar;
  background-color: rgba(255, 255, 255, 0.5);
  backdrop-filter: blur(10px);
  box-shadow: 2px 0px 5px rgba(0, 0, 0, 0.2);
  padding: 20px;
`;
const ContentArea = styled.main`
  grid-area: content;
  background-color: white;
  padding: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  box-shadow: 0 0 5px rgba(0, 0, 0, 0.1);
`;
const stackStyle: IStackStyles = {
  root: {
    width: 300,
    margin: '0 auto',
    padding: 10,
  }
};
const buttonStyle = {
  root: {
    margin: '10px 0'
  }
};

const App: React.FC = () => {
  const component = useMicroFrontend('http://localhost:5050/micro-frontend.js');

  useEffect(() => {
    if (component) {
      component.mount('micro-frontend');
    }

    return () => {
      if (component) {
        component.unmount('micro-frontend')
      }
    };
  });

  return (
    <Layout>
      <Header>
        ホストアプリケーション
      </Header>

      <Sidebar>
        <Text variant="large">
          Menu
        </Text>
        <Stack styles={stackStyle}>
          <DefaultButton text="Page1" styles={buttonStyle} />
          <DefaultButton text="リンク2" styles={buttonStyle} />
        </Stack>
      </Sidebar>

      <ContentArea>
        {component ? (
          <div id='micro-frontend' />
        ) : (
          <Spinner size={SpinnerSize.large} label='Loading...' />
        )}
      </ContentArea>
    </Layout>
  );
};

export default App;
