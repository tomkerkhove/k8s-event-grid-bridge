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

## Subscribing to events in Azure Event Grid topic

🚧 WIP

- Create Logic App
- Create subscription

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
