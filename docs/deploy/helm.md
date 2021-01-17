---
layout: page
title: Helm
parent: Deploy
---

# Helm

[![Artifact HUB](https://img.shields.io/endpoint?url=https://artifacthub.io/badge/repository/k8s-event-grid-bridge)](https://artifacthub.io/packages/search?repo=k8s-event-grid-bridge)

We allow you to easily deploy to Kubernetes through Helm.

## Adding our Helm chart registry

- Install the `k8s-event-grid-bridge` Chart repository:

```
❯ helm repo add k8s-event-grid-bridge https://k8s-event-grid-bridge.tomkerkhove.be/chart-registry
"k8s-event-grid-bridge" has been added to your repositories
```

- Refresh your local chart repositories:

```
❯ helm repo update
Hang tight while we grab the latest from your chart repositories...
...Successfully got an update from the "k8s-event-grid-bridge" chart repository
Update Complete. ⎈ Happy Helming!⎈
```

- If all goes well you should be able to list all `k8s-event-grid-bridge` charts:

```
❯ helm search hub k8s-event-grid-bridge
URL                                                     CHART VERSION   APP VERSION     DESCRIPTION
https://hub.helm.sh/charts/k8s-event-grid-bridg...      0.1.0           0.1.0           A simple event bridge for Kubernetes native eve...
```

## Installing the chart

You can easily install our Kubernetes Event Grid Bridge as following:

```
❯ helm install k8s-event-grid-bridge \
               k8s-event-grid-bridge/k8s-event-grid-bridge \
               --set azure.storage.connectionString='<storage-connection-string>' \
               --set azure.eventGrid.topicUri='<event-grid-uri>' \
               --set azure.eventGrid.key='<event-grid-auth-key>'
```

Our Helm chart provides a variety of configuration options which you can explore in our full [values file](https://github.com/tomkerkhove/k8s-event-grid-bridge/blob/main/charts/k8s-event-grid-bridge/values.yaml) to see all configurable values.
