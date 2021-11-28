$batchAccountName = '[Your Azure Batch account name]'
$resourceGroupName = '[Your resource group name]'
$poolId = 'wc-pool'

az batch account login `
    --name $batchAccountName `
    --resource-group $resourceGroupName `
    --shared-key-auth

az batch node list `
    --pool-id $poolId `
    --query '[].{id:id, state:state}' `
    -o tsv