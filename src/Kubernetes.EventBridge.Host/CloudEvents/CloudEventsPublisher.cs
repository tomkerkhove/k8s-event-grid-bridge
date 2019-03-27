using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using GuardNet;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Kubernetes.EventBridge.Host.CloudEvents
{
    public class CloudEventsPublisher
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger _logger;

        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy =
            Policy<HttpResponseMessage>.HandleResult(response => response.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(retryCount: 5,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(x: 2, y: retryAttempt)));

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

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(uriString: "http://mockbin.org/bin/a793f881-840c-4708-8977-ccc6d81a2464"))
            {
                Content = content
            };
            
            var pushResponse = await _retryPolicy.ExecuteAsync(async () => await _httpClient.SendAsync(httpRequest));

            if (pushResponse.StatusCode == HttpStatusCode.OK || pushResponse.StatusCode == HttpStatusCode.Accepted)
            {
                _logger.LogInformation($"Event '{cloudEvent.Id}' was forwarded to event topic uri '{TopicEndpointUri}'");
            }
            else
            {
                _logger.LogError($"Failed to forward event '{cloudEvent.Id}' to event topic uri '{TopicEndpointUri}'");
            }
        }
    }
}