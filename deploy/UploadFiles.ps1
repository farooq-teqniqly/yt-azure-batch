$storageAccountName = 'ytdevwestus2sto'
$storageAccountKey = '+q3EfbjErAKFyra1GTL6p4ReDbnpq8JkRWvfqw8yoc7ZQMeSpbfYkAK1HcfGlXqZ/K5/9q+QK2OK4KSwNOrV7Q=='


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