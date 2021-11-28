$batchAccountName = '[Your Azure Batch account name]'
$resourceGroupName = '[Your resource group name]'
$applicationName = 'wc'
$packageVersionName = 'latest'

$ccPath = Join-Path -Path $pwd -ChildPath 'WordCounter'
$wcProjectPath = Join-Path -Path $ccPath -ChildPath 'WordCounter.csproj'
$wcPublishOutputPath = Join-Path -Path $ccPath -ChildPath 'publish'

dotnet publish `
    $wcProjectPath `
    -o $wcPublishOutputPath `
    -c Release `
    -r win-x64 `
    --self-contained

$zipFileName = Join-Path -Path $wcPublishOutputPath -ChildPath 'wc.zip'

tar.exe `
    -v `
    -a `
    -c `
    -f $zipFileName `
    -C $wcPublishOutputPath `
    --exclude *.zip `
    *.*

az batch account login `
    --name $batchAccountName `
    --resource-group $resourceGroupName `
    --shared-key-auth

az batch application create `
    --resource-group $resourceGroupName `
    --name $batchAccountName `
    --application-name $applicationName

az batch application package delete `
    --application-name $applicationName `
    --name $batchAccountName `
    --resource-group $resourceGroupName `
    --version-name $packageVersionName `
    --yes

az batch application package create `
    --resource-group $resourceGroupName `
    --name $batchAccountName `
    --application-name $applicationName `
    --package-file $zipFileName `
    --version-name $packageVersionName

az batch application set `
    --resource-group $resourceGroupName `
    --name $batchAccountName `
    --application-name $applicationName `
    --default-version $packageVersionName

$removePublishFilesPattern = Join-Path -Path $wcPublishOutputPath -ChildPath '*.*'
Remove-Item $removePublishFilesPattern


