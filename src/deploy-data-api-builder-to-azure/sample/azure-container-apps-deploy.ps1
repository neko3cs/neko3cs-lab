#Requires -PSEdition Core
$ErrorActionPreference = 'Stop'

# 設定値
$RESOURCE_GROUP = "sample-dab-api"
$STORAGE_ACCOUNT = "samplestorage" + (Get-Random -Minimum 10000 -Maximum 100000)
$LOCATION = "japaneast"
$LOG_ANALYTICS_WORKSPACE = "sample-loganalytics-ws"
$CONTAINERAPPS_ENVIRONMENT = "dm-dab-aca-env"
$CONTAINERAPPS_APP_NAME = "dm-dab-aca-app"
$DAB_CONFIG_FILE = "./dab-config.json"
$DATABASE_CONNECTION_STRING = '{ConnectionString}'
$FILE_SHARE = "dabconfig"
$DAB_CONFIG_FILE_NAME = "dab-config.json"
$DABCONFIGFOLDER = "./${FILE_SHARE}/${DAB_CONFIG_FILE_NAME}"

# デプロイ処理
if (Test-Path log.txt) {
  Remove-Item -Path log.txt -Force
}
Write-Output "creating resource group '$RESOURCE_GROUP'" | Tee-Object -Append -FilePath log.txt
az group create `
  --name $RESOURCE_GROUP `
  --location $LOCATION `
  -o json >> log.txt

Write-Output "creating storage account: '$STORAGE_ACCOUNT'" | Tee-Object -Append -FilePath log.txt
az storage account create `
  --name $STORAGE_ACCOUNT `
  --resource-group $RESOURCE_GROUP `
  --location $LOCATION `
  --sku Standard_LRS `
  --allow-blob-public-acces false `
  -o json >> log.txt

Write-Output "retrieving storage connection string" | Tee-Object -Append -FilePath log.txt
$STORAGE_CONNECTION_STRING = $(az storage account show-connection-string `
    --name $STORAGE_ACCOUNT `
    -g $RESOURCE_GROUP `
    -o tsv)

Write-Output 'creating file share' | Tee-Object -Append -FilePath log.txt
az storage share create `
  -n $FILE_SHARE `
  --connection-string $STORAGE_CONNECTION_STRING `
  -o json >> log.txt

Write-Output "uploading configuration file '$DAB_CONFIG_FILE'" | Tee-Object -Append -FilePath log.txt
az storage file upload `
  --source $DAB_CONFIG_FILE `
  --path $DAB_CONFIG_FILE_NAME `
  --share-name $FILE_SHARE `
  --connection-string $STORAGE_CONNECTION_STRING `
  -o json >> log.txt

Write-Output "create log analytics workspace '$LOG_ANALYTICS_WORKSPACE'" | Tee-Object -Append -FilePath log.txt
az monitor log-analytics workspace create `
  --resource-group $RESOURCE_GROUP `
  --location $LOCATION `
  --workspace-name $LOG_ANALYTICS_WORKSPACE `
  -o json >> log.txt

Write-Output "retrieving log analytics client id" | Tee-Object -Append -FilePath log.txt
$LOG_ANALYTICS_WORKSPACE_CLIENT_ID = $(az monitor log-analytics workspace show  `
    --resource-group "$RESOURCE_GROUP" `
    --workspace-name "$LOG_ANALYTICS_WORKSPACE" `
    --query customerId  `
    --output tsv) `
  -replace "\s", ""

Write-Output "retrieving log analytics secret" | Tee-Object -Append -FilePath log.txt
$LOG_ANALYTICS_WORKSPACE_CLIENT_SECRET = $(az monitor log-analytics workspace get-shared-keys `
    --resource-group "$RESOURCE_GROUP" `
    --workspace-name "$LOG_ANALYTICS_WORKSPACE" `
    --query primarySharedKey `
    --output tsv) `
  -replace "\s", ""

Write-Output "retrieving storage key" | Tee-Object -Append -FilePath log.txt
$STORAGE_KEY = $(az storage account keys list `
    -g $RESOURCE_GROUP `
    -n $STORAGE_ACCOUNT `
    --query '[0].value' `
    -o tsv)

Write-Output "creating container apps environment: '$CONTAINERAPPS_ENVIRONMENT'" | Tee-Object -Append -FilePath log.txt
az containerapp env create `
  --resource-group $RESOURCE_GROUP `
  --location $LOCATION `
  --name "$CONTAINERAPPS_ENVIRONMENT" `
  --logs-workspace-id "$LOG_ANALYTICS_WORKSPACE_CLIENT_ID" `
  --logs-workspace-key "$LOG_ANALYTICS_WORKSPACE_CLIENT_SECRET" `
  -o json >> log.txt

Write-Output "waiting to finalize the ACA environment" | Tee-Object -Append -FilePath log.txt
while ($((az containerapp env show `
        -n $CONTAINERAPPS_ENVIRONMENT `
        -g $RESOURCE_GROUP `
        --query properties.provisioningState `
        -o tsv) `
      -replace "\s", "") -ne "Succeeded") {
  Start-Sleep -Seconds 10
}

Write-Output "get ACA environment id" | Tee-Object -Append -FilePath log.txt
$CONTAINERAPPS_ENVIRONMENTID = $(az containerapp env show `
    -n "$CONTAINERAPPS_ENVIRONMENT" `
    -g "$RESOURCE_GROUP" `
    --query id `
    -o tsv) `
  -replace "\r$", ""

Write-Output "mount storage account on azure container apps environment" | Tee-Object -Append -FilePath log.txt
$RES = $(az containerapp env storage set `
    --name $CONTAINERAPPS_ENVIRONMENT `
    --resource-group $RESOURCE_GROUP `
    --storage-name $FILE_SHARE `
    --azure-file-account-name $STORAGE_ACCOUNT `
    --azure-file-account-key $STORAGE_KEY `
    --azure-file-share-name $FILE_SHARE `
    --access-mode ReadWrite)

Write-Output "creating container app : '$CONTAINERAPPS_APP_NAME' on the environment : '$CONTAINERAPPS_ENVIRONMENT'" | Tee-Object -Append -FilePath log.txt
az deployment group create `
  -g $RESOURCE_GROUP `
  -f ./dab-on-aca.bicep `
  -p appName=$CONTAINERAPPS_APP_NAME dabConfigFileName=$DAB_CONFIG_FILE_NAME mountedStorageName=$FILE_SHARE environmentId=$CONTAINERAPPS_ENVIRONMENTID connectionString="$DATABASE_CONNECTION_STRING" `
  -o json >> log.txt

Write-Output "get the azure container app FQDN" | Tee-Object -Append -FilePath log.txt
$ACA_FQDN = $(az containerapp show -n $CONTAINERAPPS_APP_NAME -g $RESOURCE_GROUP --query properties.configuration.ingress.fqdn -o tsv) -replace '[A-Z]', { $_.Value.ToLower() } -replace "\s", ""

Write-Output "you can now try out the API at the following address : https://${ACA_FQDN}/api/<your-entity-name>"
Write-Output "done" | Tee-Object -Append -FilePath log.txt
