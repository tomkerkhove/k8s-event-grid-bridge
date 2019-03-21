using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using GuardNet;
using Microsoft.Extensions.Logging;

namespace Kubernetes.EventBridge.Host.CloudEvents
{
    public class CloudEventsPublisher
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger _logger;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="topicEndpointUri">Uri of the topic to publish events to</param>
        /// <param name="logger">Logger to provide insights on publishing events</param>
        public CloudEventsPublisher(string topicEndpointUri, ILogger logger)
        {
            Guard.NotNullOrEmpty(topicEndpointUri, nameof(topicEndpointUri));
            Guard.NotNull(logger, nameof(logger));

            TopicEndpointUri = topicEndpointUri;
            _logger = logger;
        }

        /// <summary>
        ///     Uri of the topic to publish events to
        /// </summary>
        public string TopicEndpointUri { get; }

        /// <summary>
        ///     Publish Cloud Event to a given topic
        /// </summary>
        /// <param name="cloudEvent">Event to publish</param>
        public async Task Publish(CloudEvent cloudEvent)
        {
            var content = new CloudEventContent(cloudEvent, ContentMode.Structured, new JsonEventFormatter());

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, TopicEndpointUri)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(httpRequest);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"Event '{cloudEvent.Id}' was forwarded to event topic uri");
            }
            else
            {
                _logger.LogError($"Failed to forward event '{cloudEvent.Id}' to event topic");
            }
        }
    }
}