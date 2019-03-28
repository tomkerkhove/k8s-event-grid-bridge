using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using GuardNet;
using Kubernetes.EventBridge.Host.Configuration.Model;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Kubernetes.EventBridge.Host.CloudEvents
{
    public class CloudEventsPublisher
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ILogger _logger;

        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="eventTopic">Information about the event topic to publish events to</param>
        /// <param name="logger">Logger to provide insights on publishing events</param>
        public CloudEventsPublisher(EventTopicConfiguration eventTopic, ILogger logger)
        {
            Guard.NotNull(eventTopic, nameof(eventTopic));
            Guard.NotNullOrEmpty(eventTopic.Uri, nameof(eventTopic.Uri));
            Guard.NotNull(eventTopic.Resiliency, nameof(eventTopic.Resiliency));
            Guard.NotNull(eventTopic.Resiliency.Retry, nameof(eventTopic.Resiliency.Retry));
            Guard.NotNull(logger, nameof(logger));

            EventTopic = eventTopic;

            _logger = logger;
            _retryPolicy = Policy<HttpResponseMessage>.HandleResult(response => response.StatusCode == HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(eventTopic.Resiliency.Retry.Count,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(x: 2, y: retryAttempt)));
        }

        /// <summary>
        ///     Information about the event topic to publish events to
        /// </summary>
        public EventTopicConfiguration EventTopic { get; }

        /// <summary>
        ///     Publish Cloud Event to a given topic
        /// </summary>
        /// <param name="cloudEvent">Event to publish</param>
        public async Task Publish(CloudEvent cloudEvent)
        {
            var content = new CloudEventContent(cloudEvent, ContentMode.Structured, new JsonEventFormatter());

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(EventTopic.Uri))
            {
                Content = content
            };

            EnrichWithCustomHeadersIfRequired(httpRequest);

            var pushResponse = await _retryPolicy.ExecuteAsync(async () => await _httpClient.SendAsync(httpRequest));

            if (pushResponse.StatusCode == HttpStatusCode.OK || pushResponse.StatusCode == HttpStatusCode.Accepted)
            {
                _logger.LogInformation($"Event '{cloudEvent.Id}' was forwarded to event topic uri '{EventTopic}'");
            }
            else
            {
                _logger.LogError($"Failed to forward event '{cloudEvent.Id}' to event topic uri '{EventTopic}'");
            }
        }

        private void EnrichWithCustomHeadersIfRequired(HttpRequestMessage httpRequest)
        {
            if (EventTopic.CustomHeaders?.Any() == true)
            {
                foreach (KeyValuePair<string, string> customHeader in EventTopic.CustomHeaders)
                {
                    httpRequest.Headers.TryAddWithoutValidation(customHeader.Key, customHeader.Value);
                }
            }
        }
    }
}