const { defineConfig } = require('@vue/cli-service')
const CopyPlugin = require('copy-webpack-plugin');
const path = require('path');

module.exports = defineConfig({
  transpileDependencies: true,
  configureWebpack: {
    plugins: [
      new CopyPlugin({
        patterns: [
          {
            from: path.resolve(__dirname, 'src/preload.js'),
            to: path.resolve(__dirname, 'dist_electron/bundled/preload.js'),
          },
        ],
      }),
    ],
  },
})
