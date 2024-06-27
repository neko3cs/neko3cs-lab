# Deploy Virtual Machine to Azure

Azure CLIとBicepを用いたAzure Virtual MachineのIaCコードサンプルです。

## デプロイ方法

### 1. 設定ファイルの作成

`Deploy-VirtualMachine.ps1` と同一階層のフォルダーに以下内容の `.env` ファイルを作成する。

`{}` で囲われた部分は適宜、用途に合わせて修正する。

```.env
RESOURCE_GROUP={YourResourceGroupName}
LOCATION=japaneast
ADMIN_USERNAME={YourAdminUserName}
ADMIN_PASSWORD={YourAdminUserPassword}
OS_VERSION={OSVersionDefinedByAzure}
VM_SIZE={VMSizeDefinedByAzure}
VM_NAME={YourVirtualMachineName}
COMPUTER_NAME={YourComputerNameOfServer}
DISK_SIZE_GB={CDriveSize}
```

### 2. 仮想マシンのデプロイ

`Deploy-VirtualMachine.ps1` を実行する。

### 3. 日本語環境の設定

以下のスクリプトを実行する。スクリプト毎に再起動が実行されるので都度ログインして実行する。

1. `Install-JapaneseLanguagePack.ps1`
1. `Set-JapaneseLanguageCulture.ps1`

### 4. ping有効設定(任意)

サーバーへの疎通確認のために `ping` コマンドを試したい時がある。

`Enable-PingFireWallRule.ps1` を実行することで `ping` 用のファイアウォールルールを有効化することができる。

## 接続方法

以下のコマンドでサーバーのIPを確認できる。

```pwsh
az vm show -g {YourResourceGroupName} -n {YourVirtualMachineName} --show-details --output table
```

リモートデスクトップ接続を起動し、コンピューター名にFQDNかIP+":3389"を入力して接続する。

## 設定値について

以下の設定値はそれぞれ制約があるため、確認の上設定する。

### ADMIN_PASSWORD

12文字以上

### OS_VERSION

以下のコマンドを実行し、表示された情報の `sku` 列にある名称を設定する。

```pwsh
az vm image list --location japaneast --offer WindowsServer --output table
```

### VM_SIZE

以下のコマンドを実行し、表示された情報の `Name` 列にある名称を設定する。

```pwsh
az vm list-sizes --location japaneast --output table
```

条件を絞って表示する場合は以下のように一度PowerShellオブジェクトに変換して `where` で絞って実行する。

```pwsh
az vm list-sizes --location japaneast | ConvertFrom-Json | where numberOfCores -eq 4 | where memoryInMB -eq 32768 | Format-Table -AutoSize
```

※azコマンドにも `--query` オプションにより絞り込みが行えるようだが、うまく動作しないし遅いし以下の方が早かった。

設定できる条件は以下のとおりです。

|パラメーター|値|
|--|--|
|name|VMサイズの名前|
|numberOfCores|CPUコア数|
|osDiskSizeInMb|OSディスクのサイズ（MB）|
|resourceDiskSizeInMb|リソースディスクのサイズ（MB）|
|memoryInMb|メモリサイズ（MB）|
|maxDataDiskCount|データディスクの最大数|

### COMPUTER_NAME

15文字以内

## 参考文献

- [クイックスタート: Bicep ファイルを使用した Windows VM の作成 - Azure Virtual Machines | Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/virtual-machines/windows/quick-create-bicep?tabs=CLI)
  - Bicepコードのベースをお借りました
