<h1 align="center">Kubernetes Event Bridge</h1>
<p align="center">
<a href="https://app.netlify.com/sites/k8s-event-bridge-staging/deploys" rel="nofollow"><img src="https://api.netlify.com/api/v1/badges/c85c7f9a-6bb1-47bc-b04e-8bd9140edd30/deploy-status" alt="Netlify"></a>
</p>

A simple event bridge for Kubernetes native events

# Concept
Kubernetes Event Bridge will subscribe for events inside the Kubernetes cluster and forward CNCF Cloud Events to a sidecar of choice, in this case an Azure Event Grid proxy.

![Kubernetes Event Router Concept](./media/concept-event-router.png)
