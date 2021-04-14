---
layout: default
title: Scale Out Node Group
parent: Cluster Autoscaler
grand_parent: Supported Events
---

## Scale Out Node Group (Cluster Autoscaler)

![Availability Badge](https://img.shields.io/badge/Available%20Starting-v0.2.0-green.svg)

This event represents Cluster Autoscaler scaling out a node group in the cluster.

Learn more about it in the [official `cluster-autoscaler` FAQ](https://github.com/kubernetes/autoscaler/blob/master/cluster-autoscaler/FAQ.md#what-events-are-emitted-by-ca).

### Event Type

`Kubernetes.Autoscaling.ClusterAutoscaler.V1.NodeGroup.ScaleOut`

### Example

```json
{
	"specversion": "1.0",
	"type": "Kubernetes.Autoscaling.ClusterAutoscaler.NodeGroup.ScaleOut",
	"source": "http://kubernetes/autoscaling/cluster-autoscaler",
	"id": "6833c9ff-567b-4b80-9cc5-ea34963097d4",
	"time": "2021-04-11T13:00:40.539442Z",
	"subject": "/node-groups/aks-agentpool-11593772-vmss",
	"datacontenttype": "application/json",
	"data": {
		"NodeGroup": {
			"Name": "aks-agentpool-11593772-vmss",
			"SizeInfo": {
				"New": 16,
				"Old": 15,
				"Maximum": 20
			}
		}
	}
}
```
