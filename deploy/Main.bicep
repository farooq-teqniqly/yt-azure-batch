targetScope = 'subscription'

param solutionName string

@allowed([
  'dev'
  'test'
])
param environmentName string

var location = deployment().location
var baseName = '${solutionName}-${environmentName}-${location}'
var rgName = '${baseName}-rg'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: rgName
  location: location
}

module batchAccountDeploy 'BatchAccount.bicep' = {
  name: 'batchAccountDeploy'
  scope: rg
  params: {
    baseName: baseName
    location: location
  }
}

module eventHubDeploy 'EventHub.bicep' = {
  name: 'eventHubDeploy'
  scope: rg
  params: {
    baseName: baseName
    location: location
  }
}

output deploymentOutputs object = {
  resourceGroupName: rgName
  batchAccountDeployment: {
    accountName: batchAccountDeploy.outputs.deploymentOutputs.batch.accountName
    endpointUrl: batchAccountDeploy.outputs.deploymentOutputs.batch.endpointUrl
    key: batchAccountDeploy.outputs.deploymentOutputs.batch.key
  }
  storageDeployment: {
    connectionString: batchAccountDeploy.outputs.deploymentOutputs.storage.connectionString
  }
  eventHubDeployment: {
    connectionString: eventHubDeploy.outputs.deploymentOutputs.eventHub.connectionString
    name: eventHubDeploy.outputs.deploymentOutputs.eventHub.name
  }
}
