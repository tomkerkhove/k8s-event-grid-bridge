---
layout: default
title: Scale In
parent: Deployments
grand_parent: Supported Events
---

## Scale In (Deployment)

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.2.0-green.svg)

This event represents a Deployment that is scaling in.

### Event Type

`Kubernetes.Autoscaling.Deployment.V1.ScaleIn`

### Example

```json
{
   "specversion": "1.0",
   "type": "Kubernetes.Autoscaling.Deployment.V1.ScaleIn",
   "source": "http://kubernetes/core/controllers/deployment",
   "id": "5410aed4-a1a8-48a4-aab4-0417551cbbf9",
   "time": "2021-04-16T06:42:54.7839842Z",
   "subject": "/local-cluster/namespaces/monitoring/deployments/k8s-event-grid-bridge-workload",
   "datacontenttype": "application/json",
   "data": {
      "Deployment": {
         "Name": "k8s-event-grid-bridge-workload",
         "Namespace": "monitoring",
         "Labels": {
            "app": "k8s-event-grid-bridge"
         }
      },
      "ReplicaSet": {
         "Name": "k8s-event-grid-bridge-workload-76888d9cc9"
      },
      "Replicas": {
         "New": 1
      }
   }
}
```
