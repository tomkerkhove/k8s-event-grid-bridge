using System;
using k8s.Models;
using Newtonsoft.Json;

namespace Kubernetes.EventBridge.Sample.Handlers
{
    public class EventPrinter : IEventHandler
    {
        public void HandleEvent(V1Event kubernetesEvent)
        {
            Console.WriteLine($"{DateTimeOffset.UtcNow:s} - {JsonConvert.SerializeObject(kubernetesEvent)}");
        }
    }
}