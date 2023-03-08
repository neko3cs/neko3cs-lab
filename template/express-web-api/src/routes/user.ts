import express from 'express';

const router = express.Router();

router.get('/all', (req: express.Request, res: express.Response) => {
  res.json([
    { id: 1, name: 'john', age: 20 },
    { id: 2, name: 'bob', age: 30 },
    { id: 3, name: 'mary', age: 14 }
  ]);
});

export default router;