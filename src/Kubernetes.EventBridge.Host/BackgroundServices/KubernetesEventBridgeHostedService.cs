using System.Threading;
using System.Threading.Tasks;
using Kubernetes.EventBridge.Host.Configuration;
using Kubernetes.EventBridge.Host.Configuration.Model;
using Kubernetes.EventBridge.Host.Kubernetes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kubernetes.EventBridge.Host.BackgroundServices
{
    public class KubernetesEventBridgeHostedService : IHostedService
    {
        private readonly KubernetesEventWatcher _kubernetesEventWatcher;
        private readonly ILogger _logger;

        public KubernetesEventBridgeHostedService(ILogger<KubernetesEventBridgeHostedService> logger, RuntimeConfiguration runtimeConfiguration)
        {
            _logger = logger;
            
            var kubernetesNamespace = runtimeConfiguration.Kubernetes.Namespace;
            var eventTopic = runtimeConfiguration.EventTopic;
            var eventSource = runtimeConfiguration.EventInfo.Source;
            _kubernetesEventWatcher = new KubernetesEventWatcher(kubernetesNamespace, eventTopic, eventSource, logger);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Kubernetes Event Bridge");

            await _kubernetesEventWatcher.Start(cancellationToken);

            _logger.LogInformation("Kubernetes Event Bridge Started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Kubernetes Event Bridge");

            await _kubernetesEventWatcher.Stop();

            _logger.LogInformation("Kubernetes Event Bridge Stopped");
        }
    }
}