# Deploy Virtual Machine to Azure

Azure CLIとBicepを用いたAzure Virtual MachineのIaCコードサンプルです。

## デプロイ方法

### 1. 設定の見直し

`Deploy-VirtualMachine.ps1` を開き、設定用変数を見直す。

デフォルト値は以下の通り。

```
$ResourceGroup = "rg-azurevm-dev-japaneast-001"
$Location = "japaneast"
$AdminUsername = "azureuser"
$AdminPassword = "P@ssword!123"
$OSVersion = "2022-datacenter-g2"
$IsDesktop = $false
$VMSize = "Standard_D2s_v5"
$VMName = "vm-azurevm-dev-001"
$ComputerName = "VM-AZUREVM-DEV-001"
$DiskSizeGB = 256
```

`$IsDesktop` を `$true` にし、 `$OSVersion` をWindows 11のものに変更すれば、Windows 11として起動できる。

### 2. 仮想マシンのデプロイ

`Deploy-VirtualMachine.ps1` を実行する。

### 3. 日本語環境の設定

設定用スクリプトは手動でBastionのコピー&ペーストの機能を使用して持ち込む。（これが一番簡単な実現方法だった...）

以下のパスに空ファイルを作成する。

- `C:\Setup\Install-JapaneseLanguagePack.ps1`

本リポジトリにある `Install-JapaneseLanguagePack.ps1` の中身をサーバー上の `Install-JapaneseLanguagePack.ps1` にコピペする。

`C:\Setup\Install-JapaneseLanguagePack.ps1` を実行する。

実行後、自動的に再起動され、日本語化される。

> [!WARNING]
> 2024年8月現在、PowerShellの `Add-WindowsCapability` コマンドは不完全なようで、一部GUIでの対応が必要になっている。
> 設定アプリの `[Time & Language] > [Language] > [Preferred languages] > [Japanese] > [Options]` を開き、ダウンロードされていないパッケージをダウンロードする。
> ※大体Language Packがされていない。

## 接続方法

Azure Bastionを利用してログインする。

Azure BastionはAzure Portalから作成したVirtual Machineを開き、`[概要] > [接続] > [Bastion]`からログインする。

ログインに成功すると操作用のタブが開く。

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

| パラメーター         | 値                             |
| -------------------- | ------------------------------ |
| name                 | VMサイズの名前                 |
| numberOfCores        | CPUコア数                      |
| osDiskSizeInMb       | OSディスクのサイズ（MB）       |
| resourceDiskSizeInMb | リソースディスクのサイズ（MB） |
| memoryInMb           | メモリサイズ（MB）             |
| maxDataDiskCount     | データディスクの最大数         |

### COMPUTER_NAME

15文字以内

## 参考文献

- [クイックスタート: Bicep ファイルを使用した Windows VM の作成 - Azure Virtual Machines | Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/virtual-machines/windows/quick-create-bicep?tabs=CLI)
  - Bicepコードのベースをお借りました
