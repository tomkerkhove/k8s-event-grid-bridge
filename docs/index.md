---
# Feel free to add content and custom Front Matter to this file.
# To modify the layout, see https://jekyllrb.com/docs/themes/#overriding-theme-defaults

layout: home
title: Welcome
nav_order: 1
---

![Logo](./media/logo-with-name.png)

[![Artifact HUB](https://img.shields.io/endpoint?url=https://artifacthub.io/badge/repository/k8s-event-grid-bridge)](https://artifacthub.io/packages/search?repo=k8s-event-grid-bridge)

A simple event bridge for Kubernetes native events forwarding [CloudEvents v1.0](https://cloudevents.io/) compliant events to Azure Event Grid into Microsoft Azure.

![High Level Overview](/media/concept/high-level-overview.png)

The bridge is not in charge of acquiring the events from Kubernetes, but you can use tools such as [Opsgenie's Kubernetes Event Exporter](https://github.com/opsgenie/kubernetes-event-exporter) and forward them to the bridge.

[GitHub](https://github.com/tomkerkhove/k8s-event-grid-bridge){: .btn }