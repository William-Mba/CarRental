#1 Define the suscription to use for the deployment
Write-Host('::::::::::::::::::::::: Deployment started :::::::::::::::::::::::')

Write-Host('Setting account subscription...')
az account set -s "Az Subscription Main"
Write-Host('Subscription setted.')

#2 Deploy the resource group
Write-Host('Deploying resource group...')
$rgName=(az deployment sub create `
    --location "canadacentral" `
    --template-file "rg-azure-deploy.json" `
    --parameters "rg-azure-deploy.parameters.test.json" | ConvertFrom-Json).properties.parameters.rgName
Write-Host('Resource group deployed.')

#3 Deploy resources
Write-Host('Deploying resources...')
az deployment group create `
    --resource-group $rgName.value `
    --template-file "azure-deploy.json" `
    --parameters "azure-deploy.parameters.test.json"
Write-Host('Resources deployed.')
Write-Host
Write-Host('::::::::::::::::::::::: Deployment completed successfully :::::::::::::::::::::::')