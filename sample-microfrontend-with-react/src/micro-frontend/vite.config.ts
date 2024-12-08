import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  build: {
    lib: {
      entry: {
        FeatureA: './src/FeatureA.tsx',
        FeatureB: './src/FeatureB.tsx',
      },
      formats: ['es'],
    },
    rollupOptions: {
      output: {
        entryFileNames: '[name].js',
      }
    }
  },
  define: {
    // Node環境変数をエミュレート
    'process.env.NODE_ENV': JSON.stringify('production'),
  }
})
