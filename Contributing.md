Contributing to Kubernetes Event Grid Bridge
===

Thanks for helping making Kubernetes Event Grid Bridge better!

## Shipping a new version

You can easily release a new Helm chart version:

1. Update the version of the Helm chart in `Chart.yaml`
2. Package the Helm chart
```shell
$ helm package .\charts\k8s-event-grid-bridge\
Successfully packaged chart and saved it to: C:\Code\GitHub\k8s-event-grid-bridge\k8s-event-grid-bridge-0.1.0.tgz
```

3. Move the new chart to the docs folder
```shell
$ mv k8s-event-grid-bridge-*.tgz .\docs\chart-registry\
```

4. Re-index the Helm repo to add our new version
```shell
$ helm repo index .\docs\chart-registry\ --url https://k8s-event-grid-bridge.tomkerkhove.be/chart-registry
```