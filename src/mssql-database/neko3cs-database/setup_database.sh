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

docker-compose up --detach
sleep 15 & pid=$!
show_spinner $pid 'docker-compose up Done!'
sqlcmd \
  -S 'localhost' -d 'master' -U 'sa' -P "P@ssword!" \
  -Q 'DROP DATABASE IF EXISTS [Neko3csDatabase]; CREATE DATABASE [Neko3csDatabase];'
sqlcmd \
  -S 'localhost' -d 'Neko3csDatabase' -U 'sa' -P "P@ssword!" \
  -i ./SetupDatabase.sql
