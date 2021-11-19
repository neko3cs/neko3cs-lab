import * as express from 'express';

const app: express.Express = express();
app.use((req, res, next) => {
  res.header("Access-Control-Allow-Origin", "*");
  res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
  next();
});
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

const router: express.Router = express.Router();
router.get('/api/hello-world', (req, res) => {
  res.send('hello world!');
});
router.get('/api/get-test', (req, res) => {
  res.send(req.query);
});
router.post('/api/post-test', (req, res) => {
  res.send(req.body);
});
app.use(router);

app.listen(3000, () => {
  console.log('example app listening on port 3000.');
});
