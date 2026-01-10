import { Application, Router } from 'oak';

const router = new Router();
router.get('/', (context) => {
  context.response.body = 'Hello, World!';
});
router.get('/hello/:name', (context) => {
  const name = context?.params?.name;
  if (name) {
    context.response.body = `Hello, ${name}!`;
  }
});
router.get('/api/coffee', async (context) => {
  const coffee = await ((await fetch('https://api.sampleapis.com/coffee/hot')).json());
  context.response.type = 'application/json';
  context.response.body = coffee;
});

const app = new Application();
app.use(router.routes());
app.use(router.allowedMethods());
await app.listen({ hostname: 'localhost', port: 8000 });
