import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  root: 'src/client',
  server: {
    port: 3000,
    proxy: {
      '/graphql': 'http://localhost:4000',
    },
  },
});
