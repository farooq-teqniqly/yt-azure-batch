param location string
param baseName string

var containerNames = [
  'input'
]

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${baseName}-insights'
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    publicNetworkAccessForIngestion: 'Enabled'
    publicNetworkAccessForQuery: 'Enabled'
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: '${replace(baseName, '-', '')}sto'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
    accessTier: 'Hot'
  }
  resource defaultBlobService 'blobServices' existing = {
    name: 'default'
  }
}

resource blobContainers 'Microsoft.Storage/storageAccounts/blobServices/containers@2020-08-01-preview' = [for name in containerNames: {
  name: name
  parent: storageAccount::defaultBlobService
  properties:{
    publicAccess: 'Container'
  }
}]

resource batchAccount 'Microsoft.Batch/batchAccounts@2020-05-01' = {
  name: '${replace(baseName, '-', '')}ba'
  location: location
  properties: {
    autoStorage: {
      storageAccountId: storageAccount.id
    }
  }
}

output deploymentOutputs object = {
  batch: {
    accountName: batchAccount.name
    endpointUrl: 'https://${batchAccount.properties.accountEndpoint}'
    key: batchAccount.listKeys().primary
  }
  storage: {
    connectionString: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${storageAccount.listKeys().keys[0].value}'
  }
  appInsights: {
    key: appInsights.properties.InstrumentationKey
  }
}
