import express from 'express';
import cors from 'cors';
import coffee from './routes/coffee';

const app: express.Express = express();

// add middleware
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// add routes
app.use('/coffee', coffee);

// implement api method
app.get('/', (req: express.Request, res: express.Response) => {
  res.send('Hello Express with TypeScript!')
});

// listen when production
if (import.meta.env.PROD) {
  app.listen(3000);
}

export const viteNodeApp = app;