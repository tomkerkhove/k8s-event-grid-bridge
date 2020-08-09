using Dapr.AzureFunctions.Extension;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Kubernetes.EventBridge.Host
{
    public static class KubernetesEventBridgeFunction
    {
        [FunctionName("kubernetes-event-bridge")]
        public static void Run(
            [DaprBindingTrigger(BindingName = "k8s-event-bridge-bindings-kubernetes")] JObject triggerData,
            [DaprBinding(BindingName = "k8s-event-bridge-bindings-eventgrid", Operation = "create")] IAsyncCollector<DaprBindingMessage> messages,
            ILogger log)
        {
            log.LogInformation($"Kubernetes event received: {triggerData}");
        }
    }
}
