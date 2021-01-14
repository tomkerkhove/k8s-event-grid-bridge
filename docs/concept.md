---
layout: page
title: Concept
permalink: /concept/
nav_order: 2
---

Kubernetes provides a wide variety of events, giving insights on what is going on in your cluster.

However; often you want to react to those events outside of your cluster and integrate with existing applications & SaaS services.

With **Kubernetes Event Grid Bridge; you can easily forward & centralize all cluster events from various clusters into Azure Event Grid**, regardless of where your clusters are.

**All forwarded events are [CloudEvents v1.0](https://cloudevents.io/) compliant**, allowing you to integrate with any tools from the ecosystem.

![High Level Overview](/media/concept/high-level-overview.png)

By using Azure Event Grid, you providing a centralized eventing hub in your platform to which all parties can subscribe, filter and process events based on their needs; by only publishing them once.

# How does it work?

Kubernetes Event Grid Bridge is single-purpose tool that is only in charge of accepting & forwarding events to Microsoft Azure and only acts as a bridge.

It does not automatically subscribe to cluster events but  the Kubernetes community provides enough tools that can be used to trigger our bridge, such as Opsgenie's [Kubernetes Event Exporter](https://github.com/opsgenie/kubernetes-event-exporter).

![End-to-end](/media/concept/end-to-end.png)

Kubernetes provides a range of events which all comply to a generic format which can make it harder to process.

With **Kubernetes Event Grid Bridge, we are planning to provide consumer-friendly events** designed to solve user needs.

This makes it easier to build workloads that extend Kubernetes.

# Unleash the power of Kubernetes events

By forwarding all events to Azure Event Grid, consumers can easily filter on various attributes such as event type, cluster name, namespace, payload and more.

By doing this, consumers can focus on solving platform needs by only processing the information they need.

Customers typically rely on these events to:

- Create **scaling awareness** by automatically posting messages to team communication tools, such as Slack
- **Automatically open support tickets** for failing pods
- Measure & visualize custom metrics on your **(scaling) dashboard**
- ...

<br/>

![Detailed Overview](/media/concept/detailed-overview.png)
