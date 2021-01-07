using System.Threading.Tasks;
using Dapr.AzureFunctions.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Kubernetes.EventBridge.Host
{
    public class OpsgenieEventExporterFunction
    {
        [FunctionName("opsgenie-kubernetes-event-exporter")]
        public async Task Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Route = "kubernetes/events/forward")] HttpRequest request,
            [DaprBinding(BindingName = "k8s-event-bridge-bindings-eventgrid", Operation = "create")] IAsyncCollector<DaprBindingMessage> messages,
            ILogger log)
        {
            var payload = await request.ReadAsStringAsync();
            log.LogInformation($"Kubernetes event received: {payload}");
        }
    }
}
