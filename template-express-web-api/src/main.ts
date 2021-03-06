import * as express from 'express';
import { createExpressServer } from 'routing-controllers';
import "reflect-metadata";
import * as cors from 'cors';
import { GreetController } from './controllers/greet-controller';

const app: express.Express = createExpressServer({
  routePrefix: '/api',
  controllers: [
    GreetController
  ]
});

app.use(cors());  // FIXME: 'Error [ERR_HTTP_HEADERS_SENT]: Cannot set headers after they are sent to the client'
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.listen(3000, () => console.log('Now listening on: http://localhost:3000'));
