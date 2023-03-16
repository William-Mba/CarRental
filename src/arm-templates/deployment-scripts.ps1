param(
    [string]$subscriptionName,
    [string]$env
)
$environmentDisplayName = $($env.ToUpper())

Write-Host

Write-Host("::::::::::::::::::::::: Deployment started in $environmentDisplayName :::::::::::::::::::::::")

#1 Define the suscription to use for the deployment
Write-Host("Setting subscription '$subscriptionName' for $environmentDisplayName deployment ..")

az account set -s "$subscriptionName"
Write-Host('Subscription setted.')

Write-Host

#2 Deploy the resource group
Write-Host("Deploying resource group in $environmentDisplayName ..")

$rgName=(az deployment sub create `
    --location "canadacentral" `
    --template-file "rg-azure-deploy.json" `
    --parameters "rg-azure-deploy.parameters.$env.json" | ConvertFrom-Json).properties.parameters.rgName
Write-Host('Resource group deployed.')

Write-Host

#3 Deploy resources
Write-Host("Deploying resources in $environmentDisplayName ..")

az deployment group create `
    --resource-group $rgName.value `
    --template-file "azure-deploy.json" `
    --parameters "azure-deploy.parameters.$env.json"

##TODO ensured deployment has succeed before
Write-Host("::::::::::::::::::::::: Successfully deployed resources in $environmentDisplayName :::::::::::::::::::::::")
