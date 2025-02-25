#!/bin/sh -e

kubectl apply -f adventureworks.yaml

kubectl wait --for=condition=Ready pod -l app=adventureworks --timeout=60s

sqlcmd -S "$(minikube ip),31433" -U sa -P P@ssword! -d master -i ./RestoreDatabase.sql
