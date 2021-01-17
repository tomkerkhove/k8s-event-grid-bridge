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

1. Create a new resource group:

```bash
# Define variables
resourceGroupName=k8s-event-grid-bridge
location=westeurope

# Create resource group
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

2. Create an Azure Event Grid topic that uses CloudEvents v1.0 as input schema:

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

# Retrieve endpoint and key to use when publishing to the topic
endpoint=$(az eventgrid topic show --name $topicName -g $resourceGroupName --query "endpoint" --output tsv)
key=$(az eventgrid topic key list --name $topicName -g $resourceGroupName --query "key1" --output tsv)
```

## Creating an Azure Logic Apps as event handler

🚧 WIP

1. Create a file called `event-handler-workflow.json` which represents an empty workflow with request trigger

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

```bash
# Define variables
logicAppName=k8s-event-handler

# Install the Azure Logic App extension (experimental)
az extension add --name logic

# Create Azure Logic App
az logic workflow create --name $logicAppName --definition "event-handler-workflow.json" --resource-group $resourceGroupName --location $location
Command group 'logic' is experimental and not covered by customer support. Please use with discretion.
{
  "accessControl": null,
  "accessEndpoint": "https://prod-30.westeurope.logic.azure.com:443/workflows/9375b613725c4453ad515e8c14d31ded",
  "changedTime": "2021-01-17T13:50:28.377752+00:00",
  "createdTime": "2021-01-17T13:50:28.389030+00:00",
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
  "endpointsConfiguration": {
    "connector": {
      "accessEndpointIpAddresses": null,
      "outgoingIpAddresses": [ <...> ]
    },
    "workflow": {
      "accessEndpointIpAddresses": [ <...> ],
      "outgoingIpAddresses": [ <...> ]
    }
  },
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

1. Go to our even handler Azure Logic App and copy the trigger URL in the editor
2. Define our variables to integrate

```bash
# Define variables
triggerUrl="<url>"
topicId=$(az eventgrid topic show --name $topicName -g $resourceGroupName --query id --output tsv)
```

3. Create an even subscipriont on our Azure Event Grid topic

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

## Deploying Kubernetes Event Grid Bridge

You can easily install our Kubernetes Event Grid Bridge through Helm:

1. Add our [Helm registry](/deploy/helm#adding-our-helm-chart-registry)

2. Install our Kubernetes Event Grid Bridge Helm chart:

```bash
❯ helm install k8s-event-grid-bridge \
               k8s-event-grid-bridge/k8s-event-grid-bridge \
               --set azure.storage.connectionString='<storage-connection-string>' \
               --set azure.eventGrid.topicUri='<event-grid-uri>' \
               --set azure.eventGrid.key='<event-grid-auth-key>'
```

🚧 WIP as I need to add Helm output

## Deploying Opsgenie's Kubernetes Event Exporter

🚧 WIP

Since we rely on [Opsgenie's Kubernetes Event Exporter](https://github.com/opsgenie/kubernetes-event-exporter), we recommend reading their documentation on setting up the exporter ([docs](https://github.com/opsgenie/kubernetes-event-exporter#deployment)).

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
