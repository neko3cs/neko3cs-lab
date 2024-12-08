import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  build: {
    lib: {
      entry: './src/main.tsx',
      name: 'MicroFrontend',
      fileName: 'micro-frontend',
      formats: ['es'],
    },
  },
  define: {
    // Node環境変数をエミュレート
    'process.env.NODE_ENV': JSON.stringify('production'),
  }
})
