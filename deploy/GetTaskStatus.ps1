$batchAccountName = '[Your Azure Batch account name]'
$resourceGroupName = '[Your resource group name]'
$jobId = 'wc-job'

az batch account login `
    --name $batchAccountName `
    --resource-group $resourceGroupName `
    --shared-key-auth

az batch job task-counts show `
    --job-id $jobId