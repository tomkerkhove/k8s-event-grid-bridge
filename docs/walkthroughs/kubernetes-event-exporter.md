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
2. Subscribing to events in Azure Event Grid topic ([link](#subscribing-to-events-in-azure-event-grid-topic))
3. Deploying Kubernetes Event Grid Bridge ([link](#deploying-kubernetes-event-grid-bridge))
4. Deploying Opsgenie's Kubernetes Event Exporter ([link](#deploying-opsgenies-kubernetes-event-exporter))

## Creating an Azure Event Grid topic

🚧 WIP

- Create Azure event grid topic

```cli
# Give your custom topic a unique name
topicName=k8s-event-grid-bridge
location=westeurope
resourceGroupName=k8s-event-grid-bridge
tom@Azure:~$ az group create --name $resourceGroupName --location $location
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


tom@Azure:~$ az eventgrid topic create --resource-group $resourceGroupName --name $topicName --location $location --input-schema cloudeventschemav1_0
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

## Subscribing to events in Azure Event Grid topic

🚧 WIP

- Create Logic App
- Create subscription

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

```
az logic workflow create --resource-group "testResourceGroup" --location "westus" --name "testLogicApp" --definition "testDefinition.json"

az logic workflow show --resource-group "testResourceGroup" --name "testLogicApp"

# Get the resource ID of the custom topic
topicID=$(az eventgrid topic show --name $myTopic -g $resourceGroup --query id --output tsv)

# Subscribe to the custom event. Include the resource group that contains the custom topic.
az eventgrid event-subscription create \
  --source-resource-id $topicID \
  --name demoSubscription \
  --endpoint $myEndpoint
```` 

## Deploying Kubernetes Event Grid Bridge

🚧 WIP

- Deploy with Helm
  - Update helm docs to pass through `--set` instead of YAML to simplify

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
