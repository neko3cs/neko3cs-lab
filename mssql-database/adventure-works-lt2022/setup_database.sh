#!/bin/sh -e

function show_spinner {
  PID=$1
  DONE_MESSAGE=$2
  SP='\|/-'
  while ps -p $PID > /dev/null
  do
    printf "\b%c" "${SP:i++%4:1}"
    sleep 0.1
  done
  printf "\n$DONE_MESSAGE\n"
}

if [ ! -f $PWD/AdventureWorksLT2022.bak ]; then 
  curl -fsSOL https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorksLT2022.bak
fi

docker-compose up --detach
sleep 20 & pid=$!
show_spinner $pid 'docker-compose up Done!'
sqlcmd \
  -S localhost -d master -U sa -P P@ssword! \
  -i ./RestoreDatabase.sql

if echo $(uname -a) | grep -q '^Linux'; then
  WSL_IP_ADDRESS=$(ip a | grep eth0 | grep inet | grep -oP '(\d{1,3}\.){3}\d{1,3}' | head -n 1)
  echo "If you are running docker on wsl2, use this ip address for SSMS.: $WSL_IP_ADDRESS"
fi
