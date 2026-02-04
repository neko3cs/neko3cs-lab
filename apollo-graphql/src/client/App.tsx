import React, { useState } from 'react';
import { createRoot } from 'react-dom/client';
import {
  ApolloClient,
  InMemoryCache,
  ApolloProvider,
  useQuery,
  gql,
} from '@apollo/client';
import './style.css';

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

  if (loading) return (
    <div className="flex justify-center items-center h-64">
      <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-brown-600"></div>
    </div>
  );
  if (error) return <p className="text-red-500 bg-red-50 p-4 rounded">Error : {error.message}</p>;

  const { coffee } = data;

  return (
    <div className="bg-white shadow-lg rounded-xl overflow-hidden border border-gray-100">
      <div className="p-6">
        <button
          onClick={onBack}
          className="mb-6 text-sm font-medium text-gray-500 hover:text-brown-600 flex items-center transition-colors"
        >
          <span className="mr-2">←</span> 一覧に戻る
        </button>

        <div className="flex flex-col md:flex-row md:items-end justify-between mb-6 gap-4">
          <div>
            <span className="inline-block px-3 py-1 rounded-full bg-brown-50 text-brown-700 text-xs font-bold uppercase tracking-wider mb-2">
              {coffee.category}
            </span>
            <h2 className="text-3xl font-bold text-gray-900">{coffee.name}</h2>
          </div>
          <div className="text-2xl font-bold text-brown-600">
            ¥{coffee.price.toLocaleString()}
          </div>
        </div>

        <p className="text-gray-600 leading-relaxed mb-8 text-lg italic">
          "{coffee.description}"
        </p>

        <div className="grid grid-cols-1 md:grid-cols-3 gap-6 pt-6 border-t border-gray-100">
          {coffee.options.size && (
            <div>
              <h4 className="text-sm font-bold text-gray-400 uppercase mb-2">Size</h4>
              <div className="flex flex-wrap gap-2">
                {coffee.options.size.map((s: string) => (
                  <span key={s} className="px-2 py-1 bg-gray-100 rounded text-sm text-gray-700">{s}</span>
                ))}
              </div>
            </div>
          )}
          {coffee.options.milk && (
            <div>
              <h4 className="text-sm font-bold text-gray-400 uppercase mb-2">Milk</h4>
              <div className="flex flex-wrap gap-2">
                {coffee.options.milk.map((m: string) => (
                  <span key={m} className="px-2 py-1 bg-gray-100 rounded text-sm text-gray-700">{m}</span>
                ))}
              </div>
            </div>
          )}
          {coffee.options.sugar && (
            <div>
              <h4 className="text-sm font-bold text-gray-400 uppercase mb-2">Sugar</h4>
              <div className="flex flex-wrap gap-2">
                {coffee.options.sugar.map((sg: string) => (
                  <span key={sg} className="px-2 py-1 bg-gray-100 rounded text-sm text-gray-700">{sg}</span>
                ))}
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

// コンポーネント: コーヒー一覧
const CoffeeList = ({ onSelect }: { onSelect: (id: string) => void }) => {
  const { loading, error, data } = useQuery(GET_COFFEES);

  if (loading) return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
      {[...Array(6)].map((_, i) => (
        <div key={i} className="h-40 bg-gray-100 animate-pulse rounded-xl"></div>
      ))}
    </div>
  );
  if (error) return <p className="text-red-500 bg-red-50 p-4 rounded">Error : {error.message}</p>;

  return (
    <div>
      <h2 className="text-2xl font-bold text-gray-800 mb-6">Menu</h2>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
        {data.coffees.map((coffee: any) => (
          <div
            key={coffee.id}
            onClick={() => onSelect(coffee.id)}
            className="group bg-white p-5 rounded-xl border border-gray-100 shadow-sm hover:shadow-md hover:border-brown-200 transition-all cursor-pointer flex flex-col justify-between"
          >
            <div>
              <span className="text-[10px] font-bold text-brown-400 uppercase tracking-widest">{coffee.category}</span>
              <h3 className="text-lg font-bold text-gray-900 group-hover:text-brown-700 transition-colors mt-1">{coffee.name}</h3>
            </div>
            <div className="mt-4 flex justify-between items-center">
              <p className="text-brown-600 font-bold">¥{coffee.price.toLocaleString()}</p>
              <span className="text-gray-300 group-hover:text-brown-400 transition-colors">→</span>
            </div>
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
    <div className="min-h-screen bg-gray-50 py-12 px-4 sm:px-6">
      <div className="max-w-5xl mx-auto">
        <header className="text-center mb-12">
          <h1 className="text-5xl font-black text-gray-900 mb-2 tracking-tight">
            ☕ COFFEE <span className="text-brown-600">LAB</span>
          </h1>
          <p className="text-gray-500 font-medium">Apollo GraphQL Learning Project</p>
        </header>

        <main>
          {selectedId ? (
            <CoffeeDetail id={selectedId} onBack={() => setSelectedId(null)} />
          ) : (
            <CoffeeList onSelect={(id) => setSelectedId(id)} />
          )}
        </main>

        <footer className="mt-20 text-center text-gray-400 text-sm">
          <p>&copy; 2026 Coffee Learning App. Built with Apollo & Tailwind.</p>
        </footer>
      </div>
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
