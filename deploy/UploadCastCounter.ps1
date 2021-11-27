$batchAccountName = 'ytdevwestus2ba'
$resourceGroupName = 'yt-dev-westus2-rg'
$applicationName = 'cc'
$packageVersionName = 'latest'

$ccPath = Join-Path -Path $pwd -ChildPath 'CastCounter'
$ccProjectPath = Join-Path -Path $ccPath -ChildPath 'CastCounter.csproj'
$ccPublishOutputPath = Join-Path -Path $ccPath -ChildPath 'publish'

dotnet publish `
    $ccProjectPath `
    -o $ccPublishOutputPath `
    -c Release `
    -r win-x64 `
    --self-contained

$zipFileName = Join-Path -Path $ccPublishOutputPath -ChildPath 'cc.zip'

tar.exe `
    -v `
    -a `
    -c `
    -f $zipFileName `
    -C $ccPublishOutputPath `
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

$removePublishFilesPattern = Join-Path -Path $ccPublishOutputPath -ChildPath '*.*'
Remove-Item $removePublishFilesPattern


