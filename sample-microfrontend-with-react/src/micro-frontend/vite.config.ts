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
      external: [
        // ここにReactやReactDomを書かないこと
        // マイクロフロントエンド事にReactのバージョン管理するため
      ],
      output: {
        entryFileNames: '[name].js'
      }
    }
  }
})
