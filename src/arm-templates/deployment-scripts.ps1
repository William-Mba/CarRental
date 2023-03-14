param(
    [string]$Environment
)
#1 Define the suscription to use for the deployment
Write-Host("::::::::::::::::::::::: Deployment started in $Environment :::::::::::::::::::::::")

Write-Host("Setting account subscription for $Environment ...")
az account set -s "Az Subscription Main"
Write-Host('Subscription setted.')
Write-Host

#2 Deploy the resource group
Write-Host("Deploying resource group to $Environment ...")
$rgName=(az deployment sub create `
    --location "canadacentral" `
    --template-file "rg-azure-deploy.json" `
    --parameters "rg-azure-deploy.parameters.$Environment.json" | ConvertFrom-Json).properties.parameters.rgName
Write-Host('Resource group deployed.')
Write-Host

#3 Deploy resources
Write-Host("Deploying resources to $Environment environment ...")
az deployment group create `
    --resource-group $rgName.value `
    --template-file "azure-deploy.json" `
    --parameters "azure-deploy.parameters.$Environment.json"

##TODO Put this message in an if statement
Write-Host("::::::::::::::::::::::: Resources successfully deployed to $Environment :::::::::::::::::::::::")