## Upload text files to Azure Batch account storage

```powershell
az extension add --name azure-batch-cli-extensions
```

```powershell
$resourceGroup = "yt-dev-westus2-rg"
$batchAccount = "ytdevwestus2ba"

az login
az batch account login -g $resourceGroup -n $batchAccount
```

```powershell
$localPath = "C:\temp\netflix-files\*.txt"
az batch file upload --local-path $localPath --file-group input
```