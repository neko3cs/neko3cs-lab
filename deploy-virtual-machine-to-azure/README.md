# Deploy Virtual Machine to Azure

Azure CLIとBicepを用いたAzure Virtual MachineのIaCコードサンプルです。

## デプロイ方法

### 1. 設定の見直し

デプロイ先サブスクリプション・リソースグループ名・リージョンは `Deploy-VirtualMachine.ps1` のパラメータで指定する。VM本体のパラメータは `CreateWindowsVirtualMachine.bicepparam` を見直す。

`Deploy-VirtualMachine.ps1` のパラメータとデフォルト値は以下の通り。

```
$SubscriptionName          # 未指定時はAzure CLIの既定サブスクリプションにデプロイする
$ResourceGroup = "rg-azurevm-dev-japaneast-001"
$Location = "japaneast"
```

リソースグループは存在チェックを行い、無い場合のみ作成する。既に存在する場合は作成をスキップするため、リソースグループの作成権限がない環境(中央管理されたサブスクリプション等)でも、払い出し済みのリソースグループを指定すれば実行できる。

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
param deployNetworkSecurityGroup = true
param existingSubnetId = ''
```

`isDesktop` を `true` にし、 `OSVersion` をWindows 11のものに変更すれば、Windows 11として起動できる。

`deployNetworkSecurityGroup` を `false` にすると、VM用サブネット・BastionサブネットへのNSG作成をスキップする。既存の共有VNetにデプロイする場合等、NSGの作成権限がない環境向け。

`existingSubnetId` に既存サブネットのリソースIDを指定すると、VNet・BastionパブリックIP・Bastionの新規作成をスキップし、指定サブネットにVMを配置する(VNet作成権限がない環境向け)。空文字のままなら、従来通り新規にVNet・Bastionを作成する。この場合、あわせて `deployNetworkSecurityGroup` も `false` にしておくのが基本(NSGも既存VNet側で管理されているため)。リソースIDの調べ方は [EXISTING_SUBNET_ID](#existing_subnet_id) を参照。

### 2. 仮想マシンのデプロイ

`Deploy-VirtualMachine.ps1` を実行する。

```pwsh
./Deploy-VirtualMachine.ps1 -SubscriptionName <サブスクリプション名> -ResourceGroup <リソースグループ名>
```

パラメータを省略した場合はスクリプト内のデフォルト値(サブスクリプションはAzure CLIの既定)が使われる。

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

### 既存の共有VNetにデプロイする場合

`existingSubnetId` を指定してデプロイした場合、Bastionは作成されない(パブリックIPも作成できないため)。接続可否は、デプロイ先の既存VNet/サブネットに設定されているNSGルールに従う。RDP接続要件については、当該VNetを管理するネットワーク担当者に確認する。

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

### EXISTING_SUBNET_ID

サブネットの**フルリソースID**を指定する。VNet名だけでは不足で、以下のように `/subnets/<サブネット名>` まで含める必要がある(NICの `subnet.id` にそのまま渡すため)。

```
/subscriptions/<サブスクリプションID>/resourceGroups/<VNetのRG名>/providers/Microsoft.Network/virtualNetworks/<VNet名>/subnets/<サブネット名>
```

VNetのリソースグループ名・VNet名が分かっている場合は、以下でサブネット一覧とそのIDを取得する。

```pwsh
az network vnet subnet list --subscription <サブスクリプション名> --resource-group <VNetのRG名> --vnet-name <VNet名> --query "[].[name, addressPrefix, id]" --output tsv
```

> [!NOTE]
> `--output table` では `id` 列が省略されてしまうため、`tsv` か `json` を使う。

VNetがどのサブスクリプション・RGにあるか分からない場合は、Azure Resource Graphで全サブスクリプションを横断検索する(`az extension add --name resource-graph` が必要)。

```pwsh
az graph query -q "Resources | where type =~ 'microsoft.network/virtualnetworks' | mv-expand subnet=properties.subnets | project vnet=name, rg=resourceGroup, subnet=tostring(subnet.name), prefix=subnet.properties.addressPrefix, id=tostring(subnet.id)" --first 500 --output json
```

> [!NOTE]
> Resource Graphは `--output table` だと件数しか表示されないため、`json` を使う。

アクセスできるサブスクリプション名の一覧は以下で確認する。

```pwsh
az account list --all --query "[].name" --output tsv
```

#### RDP接続できるサブネットかを事前に確認する

ハブスポーク構成の共有VNetでは、NSGのRDP許可ルールの**宛先が特定のサブネットに限定されている**ことがある。同じVNet・同じNSG配下でも、選んだサブネットによってはRDPが通らない。デプロイ先を決める前に、対象サブネットが許可ルールの宛先に含まれるかを確認する。

```pwsh
$subnetId = "<サブネットのリソースID>"
$nsgId = az network vnet subnet show --ids $subnetId --query "networkSecurityGroup.id" --output tsv
az network nsg show --ids $nsgId --query "securityRules[?direction=='Inbound' && access=='Allow' && (destinationPortRange=='3389' || contains(to_string(destinationPortRanges),'3389'))].{priority:priority, name:name, dst:destinationAddressPrefix}" --output tsv
```

表示された `dst` に、デプロイ先サブネットのアドレス範囲が含まれていることを確認する。含まれていなければ、そのサブネットにVMを置いてもRDP接続できない。

> [!WARNING]
> `existingSubnetId` に指定したサブネットとVMのデプロイ先サブスクリプションが食い違わないよう注意する。`Deploy-VirtualMachine.ps1` に `-SubscriptionName` を指定しない場合、Azure CLIの既定サブスクリプションにデプロイされる。現在の既定は `az account show --query name --output tsv` で確認できる。

## 参考文献

- [クイックスタート: Bicep ファイルを使用した Windows VM の作成 - Azure Virtual Machines | Microsoft Learn](https://learn.microsoft.com/ja-jp/azure/virtual-machines/windows/quick-create-bicep?tabs=CLI)
  - Bicepコードのベースをお借りました
