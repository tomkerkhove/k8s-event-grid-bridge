---
layout: default
title: Scale Out
parent: Deployments
grand_parent: Supported Events
---

## Scale Out (Deployment)

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.2.0-green.svg)

This event represents a Deployment that is scaling out.

### Event Type

`Kubernetes.Autoscaling.Deployment.V1.ScaleOut`

### Example

```json
{
   "specversion": "1.0",
   "type": "Kubernetes.Autoscaling.Deployment.V1.ScaleOut",
   "source": "http://kubernetes/core/controllers/deployment",
   "id": "2bee1da4-d922-4459-b0f8-e789825f6bad",
   "time": "2021-04-16T06:42:49.8560883Z",
   "subject": "/local-cluster/namespaces/monitoring/deployments/k8s-event-grid-bridge-workload",
   "datacontenttype": "application/json",
   "data": {
      "deployment": {
         "name": "k8s-event-grid-bridge-workload",
         "namespace": "monitoring",
         "labels": {
            "app": "k8s-event-grid-bridge"
         }
      },
      "replicaSet": {
         "name": "k8s-event-grid-bridge-workload-76888d9cc9"
      },
      "replicas": {
         "new": 10
      }
   }
}
```
