import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'
import { VitePWA } from 'vite-plugin-pwa'

export default defineConfig(({ command }) => ({
  server: { port: 5173, strictPort: true },
  preview: { port: 4173, strictPort: true },
  plugins: [react(), VitePWA({
    registerType: 'autoUpdate',
    devOptions: { enabled: true },
    includeAssets: ['favicon.svg'],
    manifest: {
      id: '/',
      name: 'PWA LAB - インストール実験',
      short_name: 'PWA LAB',
      description: 'PWA のインストールとオフライン動作を確かめるアプリ',
      theme_color: '#12243d',
      background_color: '#eef2f6',
      lang: 'ja',
      display: 'standalone',
      start_url: '/',
      scope: '/',
      related_applications: [{
        platform: 'webapp',
        url: '/manifest.webmanifest',
        id: command === 'serve' ? 'http://localhost:5173/' : 'http://localhost:4173/',
      }],
      icons: [
        { src: '/pwa-192.png', sizes: '192x192', type: 'image/png' },
        { src: '/pwa-512.png', sizes: '512x512', type: 'image/png' },
        { src: '/pwa-512.png', sizes: '512x512', type: 'image/png', purpose: 'maskable' },
      ],
    },
  })],
}))
