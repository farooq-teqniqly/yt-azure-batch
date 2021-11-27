$storageAccountName = 'ytdevwestus2sto'
$storageAccountKey = 'nXr3qYrJf35USHwS9IEx6edKX4G49H1sq2IiRLumD0/V7bLMy+O6X2LQd20NRrflbz42eMNdS2XJ1SUe71u3ng=='


tar.exe -x -f .\data\movie-cast-files.zip -C .\data

$sourcePath = Join-Path -Path $pwd -ChildPath 'data'

az storage blob upload-batch `
    --destination input `
    --account-name $storageAccountName `
    --account-key $storageAccountKey `
    --source $sourcePath `
    --pattern *.txt

$removeTextFilesPattern = Join-Path -Path $pwd -ChildPath 'data\*.txt'
Remove-Item $removeTextFilesPattern