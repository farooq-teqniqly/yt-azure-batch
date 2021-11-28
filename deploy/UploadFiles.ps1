$storageAccountName = '[Your Azure storage account name]'
$storageAccountKey = '[Your Azure storage account key]'


tar.exe -x -f .\data\words.zip -C .\data

$sourcePath = Join-Path -Path $pwd -ChildPath 'data'

az storage blob upload-batch `
    --destination input `
    --account-name $storageAccountName `
    --account-key $storageAccountKey `
    --source $sourcePath `
    --pattern *.txt

$removeTextFilesPattern = Join-Path -Path $pwd -ChildPath 'data\*.txt'
Remove-Item $removeTextFilesPattern