#!/bin/sh

yarn init &&
yarn add express cors express-validator node-fetch &&
yarn add --dev typescript vite vite-plugin-node @types/node @types/express @types/cors @types/node-fetch &&
mkdir ./routes &&
curl -fsSL https://raw.githubusercontent.com/neko3cs/neko3cs-lab/main/template/express-web-api/src/tsconfig.json -o ./tsconfig.json &&
curl -fsSL https://raw.githubusercontent.com/neko3cs/neko3cs-lab/main/template/express-web-api/src/vite.config.ts -o ./vite.config.ts &&
curl -fsSL https://raw.githubusercontent.com/neko3cs/neko3cs-lab/main/template/express-web-api/src/main.ts -o ./main.ts &&
curl -fsSL https://raw.githubusercontent.com/neko3cs/neko3cs-lab/main/template/express-web-api/src/routes/coffee.ts -o ./routes/coffee.ts
