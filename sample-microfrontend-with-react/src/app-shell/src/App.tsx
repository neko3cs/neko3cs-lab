import React from 'react';
import styled from 'styled-components';
import { DefaultButton, Spinner, SpinnerSize, Text } from '@fluentui/react';
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
const buttonStyle = {
  root: {
    margin: '10px 0'
  }
};
const textStyle = {
  root: {
    marginBottom: '20px'
  }
};

const App: React.FC = () => {
  const MicroFrontrendComponent = useMicroFrontend(
    'http://localhost:5050/MicroFrontrendComponent.js',
    'MicroFrontrendComponent'
  );

  return (
    <Layout>
      <Header>
        ホストアプリケーション
      </Header>

      <Sidebar>
        <Text variant="large">
          Menu
        </Text>
        <DefaultButton text="Page1" styles={buttonStyle} />
        <DefaultButton text="リンク2" styles={buttonStyle} />
      </Sidebar>

      <ContentArea>
        <Text
          variant='xxLarge'
          styles={textStyle}>
          Micro Frontend
        </Text>
        {MicroFrontrendComponent ? (
          <MicroFrontrendComponent />
        ) : (
          <Spinner size={SpinnerSize.large} label='Loading...' />
        )}
      </ContentArea>
    </Layout>
  );
};

export default App;
