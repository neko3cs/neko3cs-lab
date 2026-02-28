import { defineConfig } from 'vitest/config'

export default defineConfig({
  test: {
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
          include: ['src/renderer/src/**/*.test.{ts,tsx}'],
          name: 'renderer',
          environment: 'happy-dom',
          setupFiles: ['./src/renderer/src/setupTests.ts'],
        },
      },
    ],
  },
})
