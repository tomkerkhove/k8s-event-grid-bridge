# Kubernetes Event Bridge

A simple event bridge between [Opsgenie's Kubernetes Event Exporter](https://github.com/opsgenie/kubernetes-event-exporter) & Azure Event Grid to flow Kubernetes cluster events into Microsoft Azure.

# Concept

Coming later on.

# Supported Events

<details>
<summary><b>Generic Event</b></summary>

```json
{
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
            }
        ]
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
```
</details>

<br />

# Deployment

We provide a Kubernetes deployment YAML (`deploy\deploy-k8s-event-bridge.yaml`) that provides everything you need as a starting point.

Easily deploy our Kubernetes Event Bridge is very easy:

1. Update YAML file with your Azure Event Grid configuration
2. Deploy the YAML template:

```cli
kubectl apply -f .\deploy\deploy-k8s-event-bridge.yaml
```

## Deploying Opsgenie's Kubernetes Event Exporter

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
            - receiver: "dump"
            - receiver: "k8s-event-bridge"
    receivers:
      - name: "dump"
        file:
          path: "/dev/stdout"
      - name: "k8s-event-bridge"
        webhook:
          endpoint: "http://k8s-event-bridge:8888/api/kubernetes/events/forward"
          headers:
            User-Agent: kube-event-exporter 1.0
```

## License

This is licensed under The MIT License (MIT). Which means that you can use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the web application. But you always need to state that Tom Kerkhove is the original author of this web application.