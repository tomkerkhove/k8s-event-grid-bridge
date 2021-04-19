---
layout: default
title: Raw
parent: Supported Events
---

## Raw

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.1.0-green.svg)

This event represents a raw Kubernetes event without translation.

### Event Type

`Kubernetes.Events.Raw`

### Example

```json
{
   "specversion": "1.0",
   "type": "Kubernetes.Events.Raw",
   "source": "http://kubernetes",
   "id": "727b39dd-7ac1-4783-94f1-4a1d5de3d1da",
   "time": "2021-01-10T09:22:09.6277244Z",
   "datacontenttype": "application/json",
   "data": {
      "metadata": {
         "name": "k8s-event-bridge-workload.1656cffa3223676d",
         "namespace": "monitoring",
         "selfLink": "/api/v1/namespaces/monitoring/events/k8s-event-bridge-workload.1656cffa3223676d",
         "uid": "f5b5c92f-86c3-454f-a269-287dc1c46e62",
         "resourceVersion": "68019",
         "creationTimestamp": "2021-01-03T19:36:30Z",
         "managedFields": [{
            "manager": "kube-controller-manager",
            "operation": "Update",
            "apiVersion": "v1",
            "time": "2021-01-03T19:36:30Z"
         }]
      },
      "reason": "ScalingReplicaSet",
      "message": "Scaled up replica set k8s-event-bridge-workload-76888d9cc9 to 1",
      "source": {
         "component": "deployment-controller"
      },
      "firstTimestamp": "2021-01-03T19:36:30Z",
      "lastTimestamp": "2021-01-03T19:36:30Z",
      "count": 1,
      "type": "Normal",
      "reportingComponent": "",
      "reportingInstance": "",
      "involvedObject": {
         "kind": "Deployment",
         "namespace": "monitoring",
         "name": "k8s-event-bridge-workload",
         "uid": "4f3b68fc-126f-4df3-8961-c70d4d18f045",
         "apiVersion": "apps/v1",
         "resourceVersion": "68017",
         "labels": {
            "app": "k8s-event-bridge"
         }
      }
   }
}
```