# Deploy Virtual Machine to Azure

Azure CLIとBicepを用いたAzure Virtual MachineのIaCコードサンプルです。

## デプロイ方法

### 1. 設定の見直し

リソースグループ名・リージョンは `Deploy-VirtualMachine.ps1` の変数を、VM本体のパラメータは `CreateWindowsVirtualMachine.bicepparam` を見直す。

`Deploy-VirtualMachine.ps1` のデフォルト値は以下の通り。

```
$ResourceGroup = "rg-azurevm-dev-japaneast-001"
$Location = "japaneast"
```

`CreateWindowsVirtualMachine.bicepparam` のデフォルト値は以下の通り。

```
param adminUsername = 'azureuser'
param adminPassword = 'P@ssword!123'
param OSVersion = '2022-datacenter-g2'
param isDesktop = false
param vmSize = 'Standard_D2s_v5'
param vmName = 'azurevm-dev-001'
param computerName = 'AZUREVM-DEV-001'
param diskSizeGB = 256
param onPremisesAddressPrefix = '203.0.113.0/24'
```

`isDesktop` を `true` にし、 `OSVersion` をWindows 11のものに変更すれば、Windows 11として起動できる。

`onPremisesAddressPrefix` は、ExpressRoute/VPN等の専用線経由でオンプレミスからVMへ直接RDP接続する場合に、接続元となるオンプレミス側のアドレス範囲(CIDR)を指定する。専用線を利用しない場合も必須パラメータのため、到達し得ない適当なCIDR(例のドキュメント用アドレス`203.0.113.0/24`等)を設定しておく。

### 2. 仮想マシンのデプロイ

`Deploy-VirtualMachine.ps1` を実行する。

### 3. 日本語環境の設定

設定用スクリプトは手動でBastionのコピー&ペーストの機能を使用して持ち込む。（これが一番簡単な実現方法だった...）

以下のパスに空ファイルを作成する。

- `C:\Setup\Install-JapaneseLanguagePack.ps1`

本リポジトリにある `Install-JapaneseLanguagePack.ps1` の中身をサーバー上の `Install-JapaneseLanguagePack.ps1` にコピペする。

`C:\Setup\Install-JapaneseLanguagePack.ps1` を実行する。

実行後、自動的に再起動され、日本語化される。

> [!NOTE]
> `Add-WindowsCapability` はダウンロードサイズが大きい`Language.Speech`や`Language.OCR`等で一時的に失敗することがあるため、スクリプト内で自動リトライ(3回、15秒間隔)する。
> それでも失敗したパックがある場合は、再起動前にコンソールへ警告として一覧が出力される。その場合は、設定アプリの `[Time & Language] > [Language] > [Preferred languages] > [Japanese] > [Options]` を開き、該当パッケージを手動でダウンロードする。

## 接続方法

### Azure Bastion経由

Azure Bastionを利用してログインする。

Azure BastionはAzure Portalから作成したVirtual Machineを開き、`[概要] > [接続] > [Bastion]`からログインする。

ログインに成功すると操作用のタブが開く。

### 専用線(ExpressRoute/VPN)経由

`onPremisesAddressPrefix` に指定したアドレス範囲からは、VMのプライベートIPアドレスに対して直接RDP接続(3389番ポート)できる。Bastionを経由しないため、通常のRDPクライアントを使用する。

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

### DISK_SIZE_GB

127以上2048以下

### ONPREMISES_ADDRESS_PREFIX

ExpressRoute/VPN等の専用線経由でRDP接続を許可する、オンプレミス側のアドレス範囲(CIDR形式)。専用線を利用しない場合は、到達し得ないアドレス範囲(例: `203.0.113.0/24`)を設定しておく。

## 参考文献

- [クイックスタート: Bicep ファイルを使用した Windows VM の作成 - Azure Virtual Machines | Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/virtual-machines/windows/quick-create-bicep?tabs=CLI)
  - Bicepコードのベースをお借りました
