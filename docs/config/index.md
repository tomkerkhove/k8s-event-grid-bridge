---
layout: default
title: Configuring the runtime
---

Here is an overview of how you can configure the Kubernetes Event Bridge runtime. 

```yaml
kubernetes:
  namespace: sandbox
eventTopic:
  uri: https://k8s-event-bridge.westeurope-1.eventgrid.azure.net/api/events
  customHeaders:
   aeg-sas-key: 80Sxc%2FMslQ1gdVbqKtkKRwz0yDoE%2F%2FXGlBg%2Fo5ISgbo%3D
  resiliency:
    retry:
      count: 10
events:
  source: /subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-bridge/providers/Microsoft.EventGrid/topics/k8s-event-bridge#k8s-event-bridge
```

[&larr; back](/)
