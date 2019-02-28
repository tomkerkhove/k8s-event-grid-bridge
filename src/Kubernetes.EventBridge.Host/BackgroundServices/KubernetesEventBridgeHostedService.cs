using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Kubernetes.EventBridge.Host.BackgroundServices
{
    public class KubernetesEventBridgeHostedService : IHostedService
    {
        private readonly ILogger _logger;
        public KubernetesEventBridgeHostedService(ILogger<KubernetesEventBridgeHostedService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Kubernetes Event Bridge");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Kubernetes Event Bridge");
        }
    }
}