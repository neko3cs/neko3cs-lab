import * as ff from '@google-cloud/functions-framework';
import express, { Request, Response, NextFunction } from 'express';
import { SecretManagerServiceClient } from '@google-cloud/secret-manager';
import { Pool } from 'pg';

const app = express();
const secretClient = new SecretManagerServiceClient();

async function getSecret(secretName: string): Promise<string> {
  const [version] = await secretClient.accessSecretVersion({
    name: `projects/gcloud-serverless-web-app/secrets/${secretName}/versions/latest`,
  });
  const payload = version.payload?.data?.toString();
  if (!payload) {
    throw new Error(`Secret ${secretName} not found or empty`);
  }
  return payload;
}

// Basic Auth Middleware
async function basicAuth(req: Request, res: Response, next: NextFunction) {
  try {
    const expectedUsername = await getSecret('app-username');
    const expectedPassword = await getSecret('app-password');

    const authHeader = req.headers.authorization;
    if (!authHeader) {
      res.set('WWW-Authenticate', 'Basic realm="Example"');
      return res.status(401).send('Authentication required');
    }

    const auth = Buffer.from(authHeader.split(' ')[1], 'base64').toString().split(':');
    const username = auth[0];
    const password = auth[1];

    if (username === expectedUsername && password === expectedPassword) {
      return next();
    } else {
      return res.status(401).send('Invalid credentials');
    }
  } catch (error) {
    console.error('Auth error:', error);
    return res.status(500).send('Internal Server Error');
  }
}

// Database connection endpoint
app.get('/conn-db', basicAuth, async (req: Request, res: Response) => {
  let pool: Pool | undefined;
  try {
    const dbPassword = await getSecret('db-password');
    const dbHost = await getSecret('db-host');

    pool = new Pool({
      user: 'app-user',
      host: dbHost,
      database: 'app-db',
      password: dbPassword,
      port: 5432,
      ssl: false,
    });

    const result = await pool.query('SELECT version();');
    res.json({
      success: true,
      data: result.rows[0].version,
    });
  } catch (error: any) {
    console.error('DB error:', error);
    res.status(500).json({
      success: false,
      message: 'Failed to connect to DB',
      error: error.message,
    });
  } finally {
    if (pool) {
      await pool.end();
    }
  }
});

// For Cloud Functions
ff.http('app', app);
