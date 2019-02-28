using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using CloudNative.CloudEvents;
using k8s.Models;
using Newtonsoft.Json;

namespace Kubernetes.EventBridge.Sample.Handlers
{
    internal class CloudEventPrinter : IEventHandler
    {
        private readonly HttpClient _httpClient = new HttpClient();
        public void HandleEvent(V1Event kubernetesEvent)
        {
            var eventType = "Kubernetes.Event";
            var eventSource = new Uri("/subscriptions/0f9d7fea-99e8-4768-8672-06a28514f77e/resourceGroups/k8s-event-bridge/providers/Microsoft.EventGrid/topics/k8s-event-bridge#k8s-event-bridge", UriKind.Relative);
            var eventId = kubernetesEvent.Metadata.Uid?? Guid.NewGuid().ToString();
            var eventTime = kubernetesEvent.LastTimestamp ?? DateTime.UtcNow;

            var cloudEvent = new CloudEvent(CloudEventsSpecVersion.V0_1, eventType, eventSource, eventId, eventTime)
            {
                ContentType = new ContentType("application/json"),
                Data = kubernetesEvent
            };

            var rawCloudEvent = JsonConvert.SerializeObject(cloudEvent);
            Console.WriteLine($"{DateTimeOffset.UtcNow:s} - {rawCloudEvent}");

            var content = new CloudEventContent(cloudEvent, ContentMode.Structured, new JsonEventFormatter());

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://k8s-event-bridge.westeurope-1.eventgrid.azure.net/api/events")
            {
                Content = content,
                Headers =
                {
                    { "aeg-sas-key", "80Sxc/MslQ1gdVbqKtkKRwz0yDoE//XGlBg/o5ISgbo=" }
                }
            };

            var response = _httpClient.SendAsync(httpRequest).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine($"Event '{eventId}' was forwarded to Azure Event Grid");
            }
        }
    }
}
