# Azure Batch .NET application sample

## Create Azure resources
1. Open `deploy\deploy.ps1` and change the deployment location.
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

## Upload the WordCounter app to Azure Batch account storage
1. Open a Powershell session and go to the repository root.
2. Open `deploy\UploadWordCounter.ps1`.
3. Specify your batch account name and resource group name on lines 1 and 2 respectively.
4. Save the file.
5. Run `deploy\UploadWordCounter.ps1` to upload the CastCounter app to the storage account.

## Run the API
The API can be run via Docker.

1. Open a Powershell session and go to the repository root.
2. Open `docker\docker-compose.yml`.
3. Add the values for the settings on lines 10 through 14. These values can be found in the deployment output you copied in a previous step.
4. Save the file.
5. Run `docker\RunContainer.ps1`.
6. Wait until you see output similar to the following:
```powershell
batchapi_1  |       Now listening on: http://[::]:80
```
6. In a browser, navigate to `http://localhost:3000/swagger` and the Swagger page should render.

## Create a batch pool
1. Expand the `POST /pools` operation.
2. Click `Try it out`.
3. Add the request body below. This will create a pool with three compute nodes. The CastCounter application will be downloaded to each node.
```json
{
  "batchPoolConfiguration": {
    "id": "wc-pool",
    "targetDedicatedComputeNodes": 3
  },
  "imageReferenceConfiguration": {
    "offer": "WindowsServer",
    "publisher": "MicrosoftWindowsServer",
    "sku": "2022-Datacenter-smalldisk",
    "version": "latest"
  },
  "vmConfiguration": {
    "size": "STANDARD_A1_v2",
    "nodeAgentSkuId": "batch.node.windows amd64"
  },
  "applicationPackages": [
    {
      "id": "wc",
      "version": "latest"
    }
  ]
}
```
4. Click `Execute`.
5. Wait until the pool's state is `Steady` and all three nodes are in the `Idle` state. This will take several minutes to happen. You can periodically run `deploy\GetNodeState.ps1` to check the nodes' statuses.

## Create a batch job
1. Expand the `POST /jobs` operation.
2. Click `Try it out`.
3. Add the request body below to create a job in the pool you created earlier.
```json
{
  "jobId": "wc-job",
  "poolId": "wc-pool"
}
```
4. Click `Execute`.

## Create job tasks
1. Expand the `POST /jobs/{jobId}/tasks` operation.
2. Click `Try it out`.
3. Enter `wc-job` for the `jobId` parameter.
4. Enter the following request body. The value for the `eventHubConnectionString` attribute can be found in the Azure resource deployment output you copied in an earlier step.
```json
{
  "eventHubConnectionString": "Your event hub namespace connection string.",
  "eventHubName": "wordcounts",
  "inputContainerName": "input",
  "applicationPackage": {
    "id": "wc",
    "version": "latest"
  }
}
```

You can view the status of the tasks by running `deploy\GetTaskStatus.ps1`. When you see 1,000 tasks completed, the job is done.
```json
"taskCounts": {
    "active": 0,
    "completed": 1000,
    "failed": 0,
    "running": 0,
    "succeeded": 1000
  }
```
## Cleanup

### Docker cleanup
Press `CTRL+C` to terminate the container.

Remove the Docker image by running the following in a Powershell session:
```powershell
docker rmi -f docker_batchapi
```
### Azure cleanup
Delete your resource group by running the following in a Powershell session:
```powershell
az group delete --name [YOUR RESOURCE GROUP NAME] --yes
```