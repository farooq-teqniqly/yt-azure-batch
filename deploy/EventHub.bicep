param location string
param baseName string

var eventHubSku = 'Basic'
var eventHubNamespaceName = '${baseName}-ehns'
var authRuleResourceId = resourceId('Microsoft.EventHub/namespaces/authorizationRules', eventHubNamespaceName, 'RootManageSharedAccessKey')

resource eventHubNamespace 'Microsoft.EventHub/namespaces@2021-06-01-preview' = {
  name: eventHubNamespaceName
  location: location
  sku: {
    name: eventHubSku
    tier: eventHubSku
    capacity: 1
  }
  properties: {
    isAutoInflateEnabled: false
    maximumThroughputUnits: 0
  }
}

resource eventHub 'Microsoft.EventHub/namespaces/eventhubs@2021-06-01-preview' = {
  parent: eventHubNamespace
  name: 'wordcounts'
  properties: {
    messageRetentionInDays: 1
    partitionCount: 1
  }
}

output deploymentOutputs object = {
  eventHub: {
    connectionString: listkeys(authRuleResourceId, '2021-06-01-preview').primaryConnectionString
    name: eventHub.name
  }
}
