import { compile } from "nexe";

compile({
  input: "./dest/main.js",
  output: `./bin/HelloCommand`,
  targets: [
    'macos-10.13.0'
  ]
});
