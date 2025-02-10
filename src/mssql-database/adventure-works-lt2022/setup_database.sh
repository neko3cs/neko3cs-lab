#!/bin/sh -e

kubectl apply -f adventureworks.yaml

minikube ssh -- "sudo chmod 777 /data && cd /data && curl -fsSOL https://github.com/Microsoft/sql-server-samples/releases/download/adventureworks/AdventureWorksLT2022.bak"

# TODO: unable to open tcp connection with host '192.168.49.2:1433': dial tcp 192.168.49.2:1433: i/o timeout
sqlcmd -S "$(minikube ip)" -U sa -P P@ssword! -d master -i ./RestoreDatabase.sql
