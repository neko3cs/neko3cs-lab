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

// スキーマ定義の読み込み
const typeDefs = readFileSync(join(__dirname, 'schema.graphql'), 'utf-8');

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
