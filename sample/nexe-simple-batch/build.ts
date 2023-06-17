import { compile } from "nexe";

compile({
  input: "./dest/main.js",
  output: `./bin/NexeSimpleBatch`,
  // build: true,
  targets: [
    'mac-x64-8.9.4'
  ],
  python: "/usr/local/bin/python3"
});