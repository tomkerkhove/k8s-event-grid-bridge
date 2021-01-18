---
layout: default
title: Using Kubernetes Event Grid Bridge with Opsgenie's Kubernetes Event Exporter
parent: Walkthroughs
---

# Using Kubernetes Event Grid Bridge with Opsgenie's Kubernetes Event Exporter

In this walkthrough, we will automatically forward all Kubernetes events to an Azure Event Grid topic and subscribe to them from an Azure Logic App.

We'll use Opsgenie's Kubernetes Event Exporter to export and route events to Kubernetes Event Grid Bridge who transforms them into CloudEvents and publishes them on our Azure Event Grid topic.

![Overview](/media/walkthroughs/using-kubernetes-event-grid-bridge-with-opsgenie-kubernetes-event-exporter.png)

Here's an overview of the steps that we'll go through:

1. Creating an Azure Event Grid topic ([link](#creating-an-azure-event-grid-topic))
2. Creating an Azure Logic Apps as event handler ([link](#creating-an-azure-logic-apps-as-event-handler))
3. Subscribing to events in Azure Event Grid topic for our event handler ([link](#subscribing-to-events-in-azure-event-grid-topic-for-our-event-handler))
4. Deploying Kubernetes Event Grid Bridge ([link](#deploying-kubernetes-event-grid-bridge))
5. Deploying Opsgenie's Kubernetes Event Exporter ([link](#deploying-opsgenies-kubernetes-event-exporter))

## Creating an Azure Event Grid topic

We'll start by creating a new resource group that will contain Azure Event Grid topic to which we will forward events.

> 📝*While this walkthrough uses the Azure Cloud Shell, you can use any CLI tool, ARM or Azure portal if you prefer.*

- Create a new resource group:

```bash
# Define variables
resourceGroupName=k8s-event-grid-bridge
location=westeurope

# Create new resource group
az group create --name $resourceGroupName --location $location
{
  "id": "/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-grid-bridge",
  "location": "westeurope",
  "managedBy": null,
  "name": "k8s-event-grid-bridge",
  "properties": {
    "provisioningState": "Succeeded"
  },
  "tags": null,
  "type": "Microsoft.Resources/resourceGroups"
}
```

- Create an Azure Event Grid topic that uses CloudEvents v1.0 as input schema:

```bash
# Define variables
topicName=k8s-event-grid-bridge

# Create Azure Event Grid topic
az eventgrid topic create --name $topicName --input-schema cloudeventschemav1_0 --resource-group $resourceGroupName --location $location
{
  "endpoint": "https://k8s-event-grid-bridge.westeurope-1.eventgrid.azure.net/api/events",
  "id": "/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-grid-bridge/providers/Microsoft.EventGrid/topics/k8s-event-grid-bridge",
  "identity": {
    "principalId": null,
    "tenantId": null,
    "type": "None",
    "userAssignedIdentities": null
  },
  "inboundIpRules": null,
  "inputSchema": "CloudEventSchemaV1_0",
  "inputSchemaMapping": null,
  "location": "westeurope",
  "metricResourceId": "52ea2435-432b-4bb6-8338-00551a83103d",
  "name": "k8s-event-grid-bridge",
  "privateEndpointConnections": null,
  "provisioningState": "Succeeded",
  "publicNetworkAccess": "Enabled",
  "resourceGroup": "k8s-event-grid-bridge",
  "sku": {
    "name": "Basic"
  },
  "tags": null,
  "type": "Microsoft.EventGrid/topics"
}
```

## Creating an Azure Logic Apps as event handler

Next, we'll create an Azure Logic App that will act as an event handler.

- Create a file called `event-handler-workflow.json` which represents an empty workflow with request trigger

```json
{
    "definition": {
        "$schema": "https://schema.management.azure.com/providers/Microsoft.Logic/schemas/2016-06-01/workflowdefinition.json#",
        "actions": {},
        "contentVersion": "1.0.0.0",
        "outputs": {},
        "parameters": {},
        "triggers": {
            "manual": {
                "inputs": {
                    "schema": {}
                },
                "kind": "Http",
                "type": "Request"
            }
        }
    },
    "parameters": {}
}
```

- Install the experimental Azure Logic App extension for the Azure CLI

```bash
# Install the Azure Logic App extension (experimental)
az extension add --name logic
```

- Create a new Azure Logic App based on the workflow definition that we have created

```bash
# Define variables
logicAppName=k8s-event-handler

# Create Azure Logic App
az logic workflow create --name $logicAppName --definition "event-handler-workflow.json" --resource-group $resourceGroupName --location $location
Command group 'logic' is experimental and not covered by customer support. Please use with discretion.
{
  "accessControl": null,
  "accessEndpoint": "https://prod-30.westeurope.logic.azure.com:443/workflows/9375b613725c4453ad515e8c14d31ded",
  "changedTime": "2021-01-17T13:50:28.377752+00:00",
  "createdTime": "2021-01-17T13:50:28.389030+00:00",
  "definition": { <...> },
  "endpointsConfiguration": { <...>  },
  "id": "/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-grid-bridge/providers/Microsoft.Logic/workflows/k8s-event-handler",
  "integrationAccount": null,
  "integrationServiceEnvironment": null,
  "location": "westeurope",
  "name": "k8s-event-handler",
  "parameters": {},
  "provisioningState": "Succeeded",
  "resourceGroup": "k8s-event-grid-bridge",
  "sku": null,
  "state": "Enabled",
  "tags": null,
  "type": "Microsoft.Logic/workflows",
  "version": "08585907154571271828"
}
```

## Subscribing to events in Azure Event Grid topic for our event handler

Now that we have both our Azure Event Grid topic, we can create an event subscription to forward all events to our new event handler.

- Go to our even handler Azure Logic App and copy the trigger URL in the editor
- Store the trigger URL and topic id as variables

```bash
# Define variables
triggerUrl="<url>"
topicId=$(az eventgrid topic show --name $topicName -g $resourceGroupName --query id --output tsv)
```

- Create a new event subscription on our Azure Event Grid topic

```bash
az eventgrid event-subscription create \
  --source-resource-id $topicId \
  --name example-subscription \
  --endpoint $triggerUrl \
  [--included-event-types "Kubernetes.Events.Raw"] # Optionally filter on event type
{
  "deadLetterDestination": null,
  "deadLetterWithResourceIdentity": null,
  "deliveryWithResourceIdentity": null,
  "destination": {
    "azureActiveDirectoryApplicationIdOrUri": null,
    "azureActiveDirectoryTenantId": null,
    "endpointBaseUrl": "<redacted>",
    "endpointType": "WebHook",
    "endpointUrl": null,
    "maxEventsPerBatch": 1,
    "preferredBatchSizeInKilobytes": 64
  },
  "eventDeliverySchema": "CloudEventSchemaV1_0",
  "expirationTimeUtc": null,
  "filter": {
    "advancedFilters": null,
    "includedEventTypes": [
      "Kubernetes.Events.Raw"
    ],
    "isSubjectCaseSensitive": null,
    "subjectBeginsWith": "",
    "subjectEndsWith": ""
  },
  "id": "/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-grid-bridge/providers/Microsoft.EventGrid/topics/k8s-event-grid-bridge/providers/Microsoft.EventGrid/eventSubscriptions/example-subscription",
  "labels": null,
  "name": "example-subscription",
  "provisioningState": "Succeeded",
  "resourceGroup": "k8s-event-grid-bridge",
  "retryPolicy": {
    "eventTimeToLiveInMinutes": 1440,
    "maxDeliveryAttempts": 30
  },
  "topic": "/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-grid-bridge/providers/microsoft.eventgrid/topics/k8s-event-grid-bridge",
  "type": "Microsoft.EventGrid/eventSubscriptions"
}
```

That's it, we are ready to receive events!

## Deploying Kubernetes Event Grid Bridge

You can easily install our Kubernetes Event Grid Bridge through Helm. Before we do that, we need to create an Azure Storage Account that is required ([at least, for now](https://github.com/tomkerkhove/k8s-event-grid-bridge/issues/105)) and our Azure Event Grid topic information.

Let's get started!

- Create a new Azure Storage Account

```bash
# Define variables
storageAccountName=k8seventgridbridge

# Create a new Azure Storage Account
az storage account create \
    --name $storageAccountName \
    --resource-group $resourceGroupName \
    --location $location \
    --sku Standard_LRS
```

- Retrieve all required information as variables

```bash
# Retrieve required information for Kubernetes Event Grid Bridge to authenticate to Azure Event Grid topic & Storage
eventGridEndpoint=$(az eventgrid topic show --name $topicName -g $resourceGroupName --query "endpoint" --output tsv)
eventGridAuthKey=$(az eventgrid topic key list --name $topicName -g $resourceGroupName --query "key1" --output tsv)
storageConnectionString=$(az storage account show-connection-string  --name $storageAccountName --resource-group $resourceGroupName --query "connectionString" --output tsv)
```

- Add our [Helm registry](/deploy/helm#adding-our-helm-chart-registry)

- Install our Kubernetes Event Grid Bridge Helm chart:

```bash
❯ helm install k8s-event-grid-bridge \
               k8s-event-grid-bridge/k8s-event-grid-bridge \
               --set azure.storage.connectionString=$storageConnectionString \
               --set azure.eventGrid.topicUri=$eventGridEndpoint \
               --set azure.eventGrid.key=$eventGridAuthKey
NAME: k8s-event-grid-bridge
LAST DEPLOYED: Mon Jan 18 06:57:36 2021
NAMESPACE: default
STATUS: deployed
REVISION: 1
NOTES:
Kubernetes Event Grid Bridge is up & running!

Internal workloads can forward requests to http://k8s-event-grid-bridge.default.svc.cluster.local:8888.
```

That's it, the Kubernetes Event Grid Bridge is up & running and ready to forward events to Azure Event Grid!

## Deploying Opsgenie's Kubernetes Event Exporter

Lastly, we will deploy [Opsgenie's Kubernetes Event Exporter](https://github.com/opsgenie/kubernetes-event-exporter) to export Kubernetes events from the API Server and send them to our Kubernetes Event Grid Bridge instance!

You can deploy the Kubernetes Event Exporter through raw Kubernetes YAML files ([docs](https://github.com/opsgenie/kubernetes-event-exporter#deployment)) or use a community-based Helm chart by Bitnami.

To make it easy, we'll use Helm:

- 

https://artifacthub.io/packages/helm/bitnami/kubernetes-event-exporter --> Community, not official. Official is coming through https://github.com/opsgenie/kubernetes-event-exporter/pull/104

```yaml
image:
  registry: docker.io
  repository: opsgenie/kubernetes-event-exporter
  tag: latest
config:
  route:
    routes:
      - match:
          - receiver: "k8s-event-grid-bridge"
  receivers:
    - name: "k8s-event-grid-bridge"
      webhook:
        endpoint: "http://k8s-event-grid-bridge:8888/api/kubernetes/events/forward"
        headers:
          User-Agent: kube-event-exporter 1.0
```

helm install kubernetes-event-exporter bitnami/kubernetes-event-exporter --values config.ymlNAME: kubernetes-event-exporter
LAST DEPLOYED: Mon Jan 18 08:40:51 2021
NAMESPACE: default
STATUS: deployed
REVISION: 1
TEST SUITE: None

Here is an example configuration that you can use:

```yaml
apiVersion: v1
kind: ConfigMap
metadata:
  name: event-exporter-cfg
  namespace: monitoring
data:
  config.yaml: |
    logLevel: error
    logFormat: json
    route:
      routes:
        - match:
            - receiver: "k8s-event-grid-bridge"
    receivers:
      - name: "k8s-event-grid-bridge"
        webhook:
          endpoint: "http://k8s-event-grid-bridge:8888/api/kubernetes/events/forward"
          headers:
            User-Agent: kube-event-exporter 1.0
```
