#!/bin/bash

# buildディレクトリの作成
mkdir -p build

echo "Building server..."
go build -o build/server ./src/server

echo "Building client..."
go build -o build/client ./src/client

echo "Build complete. Binaries are in the 'build' directory."
