import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'

export default ({ command, mode }) => {
  const env = loadEnv(mode, process.cwd(), '')

  return defineConfig({
    base: '/',
    build: {
      outDir: '../wwwroot/dist',
      emptyOutDir: true,
      manifest: false,
      rollupOptions: {
        input: {
          main: './index.html',
        }
      }
    },
    plugins: [react()],
  });
}