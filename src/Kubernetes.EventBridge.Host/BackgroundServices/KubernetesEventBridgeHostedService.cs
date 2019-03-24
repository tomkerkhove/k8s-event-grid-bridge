using System.Threading;
using System.Threading.Tasks;
using Kubernetes.EventBridge.Host.Kubernetes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kubernetes.EventBridge.Host.BackgroundServices
{
    public class KubernetesEventBridgeHostedService : IHostedService
    {
        private readonly KubernetesEventWatcher _kubernetesEventWatcher;
        private readonly ILogger _logger;

        public KubernetesEventBridgeHostedService(ILogger<KubernetesEventBridgeHostedService> logger, IConfiguration configuration)
        {
            _logger = logger;

            var topicEndpointUri = configuration.GetValue<string>(key: "TOPIC_URI");
            var eventSource = configuration.GetValue<string>(key: "EVENT_SOURCE");
            _kubernetesEventWatcher = new KubernetesEventWatcher(topicEndpointUri, eventSource, logger, configuration);
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