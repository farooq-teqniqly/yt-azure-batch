# Azure Batch .NET application sample

## Create Azure resources
1. Open `deploy\deploy.ps1` and change the deployment location if desired.
2. Open `deploy\parameters.dev.json` and change the value of the `solutioName` parameter to a unique string.
3. Save the file.
4. Open a Powershell session and go to the repository root.
5. Run `deploy\deploy.ps1` which starts the deployment.
6. When the deployment completes, copy the `outputs` JSON object as you will need these values later.

## Upload text files to Azure Batch account storage

1. Open a Powershell session and go to the repository root.
2. Open `deploy\UploadFiles.ps1`.
3. Specify your storage account name and key on lines 1 and 2 respectively. The account name and key can be found in the deployment output you copied in a previous step.
4. Save the file.
5. Run `deploy\UploadFiles.ps1` to upload the files to the storage account. 1,000 files should be uploaded to the container named `input`.

## Upload the CastCounter app to the Azure Batch account storage
1. Open a Powershell session and go to the repository root.
2. Open `deploy\UploadFiles.ps1`.
3. Specify your batch account name and resource group name on lines 1 and 2 respectively.
4. Save the file.
5. Run `deploy\UploadCastCounter.ps1` to upload the CastCounter app to the storage account.

## Run the API
The API can be run via Docker.

1. Open a Powershell session and go to the repository root.
2. Run `BuildContainer.ps1`.
3. When the container finishes building, run the following to start the container:
```powershell
docker run -p 3000:80 --name batchapi  -d yt/az-batch-api 
```
4. In a browser, navigate to `http://localhost:3000/swagger` and the Swagger page should render.

## Cleanup

### Docker cleanup
Remove the API container by running the following in a Powershell session:
```powershell
docker rm -f batchapi
```

Remove the Docker image by running the following in a Powershell session:
```powershell
docker rmi -f yt/az-batch-api
```

### Azure cleanup
Delete your resource group by running the following in a Powershell session:
```powershell
az group delete --name [YOUR RESOURCE GROUP NAME] --yes
```