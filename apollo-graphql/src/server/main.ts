import { ApolloServer } from '@apollo/server';
import { startStandaloneServer } from '@apollo/server/standalone';
import { readFileSync } from 'fs';
import { join } from 'path';
import { fileURLToPath } from 'url';

// JSONデータの読み込み
const __dirname = fileURLToPath(new URL('.', import.meta.url));
const coffeeData = JSON.parse(
  readFileSync(join(__dirname, 'data/coffee.json'), 'utf-8')
);

// スキーマ定義
const typeDefs = `#graphql
  """
  コーヒーのカスタマイズオプション
  """
  type Options {
    size: [String]
    milk: [String]
    sugar: [String]
  }

  """
  コーヒー商品情報
  """
  type Coffee {
    id: ID!
    name: String!
    price: Int!
    category: String!
    description: String
    options: Options
  }

  type Query {
    """
    全てのコーヒー商品を取得します
    """
    coffees: [Coffee]

    """
    IDを指定して特定のコーヒー商品を取得します
    """
    coffee(id: ID!): Coffee
  }
`;

// リゾルバーの実装
const resolvers = {
  Query: {
    coffees: () => coffeeData,
    coffee: (_parent: any, args: { id: string }) => 
      coffeeData.find((c: any) => c.id === args.id),
  },
};

// サーバーの起動
const server = new ApolloServer({
  typeDefs,
  resolvers,
});

const { url } = await startStandaloneServer(server, {
  listen: { port: 4000 },
});

console.log(`🚀 Server ready at ${url}`);
