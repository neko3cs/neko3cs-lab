import { defineConfig } from 'vitest/config'

export default defineConfig({
  test: {
    coverage: {
      provider: 'v8',
      reporter: ['text', 'json', 'html'],
      include: ['src/**/*'],
      exclude: [
        'src/**/*.test.ts',
        'src/**/*.test.tsx',
        'src/**/__mocks__/**',
        'src/**/*.d.ts',
        'src/**/*.css',
        'src/**/*.svg',
        'src/**/*.png',
      ],
    },
    projects: [
      {
        test: {
          globals: true,
          include: ['src/main/**/*.test.ts'],
          name: 'main',
          environment: 'node',
          setupFiles: ['./src/main/setupTests.ts'],
        },
      },
      {
        test: {
          globals: true,
          include: ['src/preload/**/*.test.ts'],
          name: 'preload',
          environment: 'happy-dom',
        },
      },
      {
        test: {
          globals: true,
          include: ['src/renderer/src/**/*.test.{ts,tsx}'],
          name: 'renderer',
          environment: 'happy-dom',
          setupFiles: ['./src/renderer/src/setupTests.ts'],
        },
      },
    ],
  },
})
