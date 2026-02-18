import { serve } from '@hono/node-server'
import { Hono } from 'hono'
import { Client } from 'pg'

const app = new Hono()

app.get('/healthCheck', (c) => c.json({ status: 'ok' }))

app.get('/conn-db', async (c) => {
  const client = new Client({
    host: process.env.DB_HOST,
    user: process.env.DB_USER,
    password: process.env.DB_PASSWORD,
    database: process.env.DB_NAME || 'app',
    port: parseInt(process.env.DB_PORT || '5432'),
    ssl: { rejectUnauthorized: false },
  })

  try {
    await client.connect()
    const res = await client.query('SELECT version();')
    await client.end()
    return c.json({
      status: 'success',
      version: res.rows[0].version
    })
  } catch (err) {
    console.error(err)
    return c.json({
      status: 'error',
      message: err instanceof Error ? err.message : String(err)
    }, 500)
  }
})

const port = 3000
console.log(`Server is running on port ${port}`)

serve({
  fetch: app.fetch,
  port
})
