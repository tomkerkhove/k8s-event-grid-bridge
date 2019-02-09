# Kubernetes Event Bridge
A simple event bridge for Kubernetes native events

# Concept
Kubernetes Event Bridge will subscribe for events inside the Kubernetes cluster and forward CNCF Cloud Events to a sidecar of choice, in this case an Azure Event Grid proxy.

![Kubernetes Event Router Concept](./media/concept-event-router.png)
