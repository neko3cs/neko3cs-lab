param(
    [string]$resourceGroupName = "rg-azure-serverless-web-api",
    [string]$location = "japaneast"
)

# Resource Group の作成
az group create --name $resourceGroupName --location $location

# Bicep のデプロイ
az deployment group create `
    --resource-group $resourceGroupName `
    --template-file ./main.bicep

# アプリケーションのビルドとデプロイ (簡略化)
# cd ../src-app/AzureServerlessWebApi
# func azure functionapp publish <func-app-name>
