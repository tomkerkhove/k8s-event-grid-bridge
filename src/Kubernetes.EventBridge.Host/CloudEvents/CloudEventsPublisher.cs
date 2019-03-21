using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using GuardNet;

namespace Kubernetes.EventBridge.Host.CloudEvents
{
    public class CloudEventsPublisher
    {
        private readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="topicEndpointUri">Uri of the topic to publish events to</param>
        public CloudEventsPublisher(string topicEndpointUri)
        {
            Guard.NotNullOrEmpty(topicEndpointUri, nameof(topicEndpointUri));

            TopicEndpointUri = topicEndpointUri;
        }

        /// <summary>
        /// Uri of the topic to publish events to
        /// </summary>
        public string TopicEndpointUri { get; }

        /// <summary>
        /// Publish Cloud Event to a given topic
        /// </summary>
        /// <param name="cloudEvent">Event to publish</param>
        public async Task Publish(CloudEvent cloudEvent)
        {
            var content = new CloudEventContent(cloudEvent, ContentMode.Structured, new JsonEventFormatter());

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUri: TopicEndpointUri)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(httpRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Console.WriteLine($"Event '{cloudEvent.Id}' was forwarded to Azure Event Grid");
            }
        }
    }
}