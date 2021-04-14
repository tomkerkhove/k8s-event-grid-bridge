![Logo](https://raw.githubusercontent.com/tomkerkhove/k8s-event-grid-bridge/main/docs/media/logo-with-name-small.png)

A simple event bridge for Kubernetes native events forwarding [CloudEvents v1.0](https://cloudevents.io/) compliant events to Azure Event Grid into Microsoft Azure.

The bridge is not in charge of acquiring the events from Kubernetes, but you can use tools such as [Opsgenie's Kubernetes Event Exporter](https://github.com/opsgenie/kubernetes-event-exporter) and forward them to the bridge.

[![Artifact HUB](https://img.shields.io/endpoint?url=https://artifacthub.io/badge/repository/k8s-event-grid-bridge)](https://artifacthub.io/packages/search?repo=k8s-event-grid-bridge)

## Configuration
azure:
  storage:
    connectionString:
  eventGrid:
    topicUri: 
    key:

The following table lists the configurable parameters of the Helm chart and
their default values.

| Parameter                  | Description                                                                                                                                                     | Default           |
| :------------------------- | :-------------------------------------------------------------------------------------------------------------------------------------------------------------- | :---------------- |
| `azure.eventGrid.key`      | Authentication key for Azure Event Grid.                                                                                                                        | ``                |
| `azure.eventGrid.topicUri` | Uri of the Azure Event Grid topic to send events to.                                                                                                            | ``                |
| `kubernetes.cluster.name`             | Name of the Kubernetes cluster. This will be used in the emitted events and observability to support multi-cluster events into a single Azure Event Grid topic. | `unknown-cluster` |
