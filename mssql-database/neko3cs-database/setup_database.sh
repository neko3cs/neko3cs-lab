#!/bin/sh

docker-compose up --detach
sqlcmd \
  -S 'localhost' -d 'master' -U 'sa' -P "P@ssword!" \
  -Q 'DROP DATABASE IF EXISTS [Neko3csDatabase]; CREATE DATABASE [Neko3csDatabase];'
sqlcmd \
  -S 'localhost' -d 'Neko3csDatabase' -U 'sa' -P "P@ssword!" \
  -i ./SetupDatabase.sql
