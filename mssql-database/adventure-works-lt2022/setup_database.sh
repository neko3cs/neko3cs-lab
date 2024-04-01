#!/bin/sh

if [ ! -f $PWD/AdventureWorksLT2022.bak ]; then 
  curl -fsSOL https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorksLT2022.bak
fi

docker-compose up --detach
sqlcmd \
  -S localhost -d master -U sa -P P@ssword! \
  -i ./RestoreDatabase.sql

WSL_IP_ADDRESS=ip a | grep eth0 | grep inet | grep -oP '(\d{1,3}\.){3}\d{1,3}' | head -n 1
echo "If you are running docker on wsl2, use this ip address for SSMS.: $WSL_IP_ADDRESS"

