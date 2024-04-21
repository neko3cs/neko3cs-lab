# Azure Virtual Machine IaC Deploy

Azure CLIとBicepを用いたAzure Virtual MachineのIaCコードサンプルです。

## 使い方

`Deploy-VirtualMachine.ps1` と同一階層に以下のような `.env` ファイルを用意して実行する。

```.env
RESOURCE_GROUP=YourResourceGroupName
LOCATION=japaneast
ADMIN_USERNAME=LocalAdmin
ADMIN_PASSWORD=YourPassword
OS_VERSION=2022-datacenter-g2
VM_NAME=YourVMName
DISK_SIZE_GB=512
```

## 参考文献

- [クイックスタート: Bicep ファイルを使用した Windows VM の作成 - Azure Virtual Machines | Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/virtual-machines/windows/quick-create-bicep?tabs=CLI)
  - Bicepコードのベースをお借りしました
