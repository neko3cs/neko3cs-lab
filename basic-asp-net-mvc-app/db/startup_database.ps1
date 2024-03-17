#!/usr/bin/env pwsh

docker pull mcr.microsoft.com/mssql/server:latest

docker run `
    -e 'ACCEPT_EULA=Y' `
    -e 'SA_PASSWORD=P@ssword!' `
    -p 1434:1433 `
    --name sql1 `
    -d mcr.microsoft.com/mssql/server:latest
