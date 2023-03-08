import express from 'express';
import cors from 'cors';
import user from './routes/user';

const app: express.Express = express();

// add middleware
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// add routes
app.use('/user', user);

// implement api method
app.get('/', (req: express.Request, res: express.Response) => {
  res.send('Hello Express with TypeScript!')
});

// listen when production
if (import.meta.env.PROD) {
  app.listen(3000);
}

export const viteNodeApp = app;