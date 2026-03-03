param(
    [string]$resourceGroupName = "rg-azure-serverless-web-api",
    [string]$location = "japaneast"
)

# Resource Group の作成
Write-Host "Creating Resource Group: $resourceGroupName"
az group create --name $resourceGroupName --location $location

# Bicep のデプロイ
Write-Host "Deploying Bicep template..."
$deployment = az deployment group create `
    --resource-group $resourceGroupName `
    --template-file ./main.bicep `
    --query "properties.outputs" -o json | ConvertFrom-Json

# Bicep のアウトプットから情報を取得
$keyVaultName = $deployment.keyVaultName.value
$sqlServerFqdn = $deployment.sqlServerFqdn.value
$sqlDbName = $deployment.sqlDbName.value
$sqlAdminLogin = $deployment.sqlAdminLogin.value

# 接続文字列の作成 (パスワードは main.bicep に固定したものを一時的に使用)
# 本来は動的に設定するか、Bicep 内で Key Vault に直接登録するのがベストですが、
# 今回は Deploy.ps1 からの登録要件に対応します。
$sqlPassword = "Password123!"
$connectionString = "Server=tcp:$sqlServerFqdn,1433;Initial Catalog=$sqlDbName;User ID=$sqlAdminLogin;Password=$sqlPassword;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

# Key Vault へのシークレット登録
Write-Host "Registering DbConnectionString in Key Vault: $keyVaultName"
az keyvault secret set --vault-name $keyVaultName --name "DbConnectionString" --value $connectionString

Write-Host "Deployment completed successfully."
Write-Host "Next Step: cd ../src-app/AzureServerlessWebApi && func azure functionapp publish az-srv-api-func"
