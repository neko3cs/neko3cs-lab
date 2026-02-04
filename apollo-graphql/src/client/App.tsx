import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import { 
  ApolloClient, 
  InMemoryCache, 
  ApolloProvider, 
  useQuery, 
  gql,
} from '@apollo/client';

// Apollo Clientの設定
const client = new ApolloClient({
  uri: 'http://localhost:4000',
  cache: new InMemoryCache(),
});


// GraphQL Queries
const GET_COFFEES = gql`
  query GetCoffees {
    coffees {
      id
      name
      price
      category
    }
  }
`;

const GET_COFFEE_DETAIL = gql`
  query GetCoffeeDetail($id: ID!) {
    coffee(id: $id) {
      id
      name
      price
      category
      description
      options {
        size
        milk
        sugar
      }
    }
  }
`;

// コンポーネント: コーヒー詳細
const CoffeeDetail = ({ id, onBack }: { id: string, onBack: () => void }) => {
  const { loading, error, data } = useQuery(GET_COFFEE_DETAIL, {
    variables: { id },
  });

  if (loading) return <p>Loading detail...</p>;
  if (error) return <p>Error : {error.message}</p>;

  const { coffee } = data;

  return (
    <div style={{ padding: '20px', border: '1px solid #ccc', borderRadius: '8px' }}>
      <button onClick={onBack}>← 一覧に戻る</button>
      <h2>{coffee.name}</h2>
      <p style={{ color: '#666' }}>{coffee.category}</p>
      <p>{coffee.description}</p>
      <h3>価格: ¥{coffee.price}</h3>

      <h4>カスタマイズオプション:</h4>
      <ul>
        {coffee.options.size && <li>サイズ: {coffee.options.size.join(', ')}</li>}
        {coffee.options.milk && <li>ミルク: {coffee.options.milk.join(', ')}</li>}
        {coffee.options.sugar && <li>砂糖: {coffee.options.sugar.join(', ')}</li>}
      </ul>
    </div>
  );
};

// コンポーネント: コーヒー一覧
const CoffeeList = ({ onSelect }: { onSelect: (id: string) => void }) => {
  const { loading, error, data } = useQuery(GET_COFFEES);

  if (loading) return <p>Loading coffees...</p>;
  if (error) return <p>Error : {error.message}</p>;

  return (
    <div>
      <h2>コーヒーメニュー</h2>
      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fill, minmax(200px, 1fr))', gap: '10px' }}>
        {data.coffees.map((coffee: any) => (
          <div
            key={coffee.id}
            onClick={() => onSelect(coffee.id)}
            style={{
              padding: '15px',
              border: '1px solid #eee',
              borderRadius: '4px',
              cursor: 'pointer',
              backgroundColor: '#f9f9f9'
            }}
          >
            <h3 style={{ margin: '0 0 10px 0' }}>{coffee.name}</h3>
            <p style={{ margin: 0 }}>¥{coffee.price}</p>
            <small style={{ color: '#888' }}>{coffee.category}</small>
          </div>
        ))}
      </div>
    </div>
  );
};

// メインアプリケーション
const App = () => {
  const [selectedId, setSelectedId] = useState<string | null>(null);

  return (
    <div style={{ maxWidth: '800px', margin: '0 auto', fontFamily: 'sans-serif' }}>
      <h1>☕ Coffee Learning App</h1>
      <hr />
      {selectedId ? (
        <CoffeeDetail id={selectedId} onBack={() => setSelectedId(null)} />
      ) : (
        <CoffeeList onSelect={(id) => setSelectedId(id)} />
      )}
    </div>
  );
};

// レンダリング
const container = document.getElementById('root');
if (container) {
  const root = createRoot(container);
  root.render(
    <React.StrictMode>
      <ApolloProvider client={client}>
        <App />
      </ApolloProvider>
    </React.StrictMode>
  );
}
