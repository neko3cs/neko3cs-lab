import * as express from 'express';
import "reflect-metadata";
import { createExpressServer } from 'routing-controllers';
import { GreetController } from './controllers/greet-controller';

const app: express.Express = createExpressServer({
  routePrefix: '/api',
  controllers: [
    GreetController
  ]
});

app.use((req, res, next) => {
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  next();
});
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.listen(3000, () => console.log('Now listening on: http://localhost:3000'));
