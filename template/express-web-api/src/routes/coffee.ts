import express from 'express';
import fetch from 'node-fetch';

const router = express.Router();

router.get('/all', async (req: express.Request, res: express.Response) => {
  res.json(await (await fetch("https://api.sampleapis.com/coffee/hot")).json());
});

export default router;