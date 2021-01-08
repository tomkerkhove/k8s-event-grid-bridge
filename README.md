# Kubernetes Event Bridge

A simple event bridge between [Opsgenie's Kubernetes Event Exporter](https://github.com/opsgenie/kubernetes-event-exporter) & Azure Event Grid to flow Kubernetes cluster events into Microsoft Azure.

# Concept

Coming later on.

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