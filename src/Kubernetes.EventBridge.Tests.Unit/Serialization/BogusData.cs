using System.Collections.Generic;
using Bogus;
using Kubernetes.EventBridge.Host.Configuration.Model;

namespace Kubernetes.EventBridge.Tests.Unit.Serialization
{
    internal static class BogusData
    {
        internal static EventInfo GenerateEventInfo()
        {
            var bogusGenerator = new Faker<EventInfo>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(eventInfo => eventInfo.Source, faker => faker.Name.FirstName());

            return bogusGenerator.Generate();
        }

        internal static EventTopicConfiguration GenerateEventTopicConfig()
        {
            var resiliencyConfiguration = GenerateResiliencyConfig();
            var bogusGenerator = new Faker<EventTopicConfiguration>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(eventTopic => eventTopic.Uri, faker => faker.Internet.Url())
                .RuleFor(eventTopic => eventTopic.Resiliency, faker => resiliencyConfiguration)
                .RuleFor(eventTopic => eventTopic.CustomHeaders, faker =>
                {
                    var customHeaders = new Dictionary<string, string>
                    {
                        {faker.Name.FirstName(), faker.Name.FirstName()}
                    };
                    return customHeaders;
                });

            return bogusGenerator.Generate();
        }

        internal static KubernetesConfiguration GenerateKubernetesConfig()
        {
            var bogusGenerator = new Faker<KubernetesConfiguration>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(kubernetes => kubernetes.Namespace, faker => faker.Name.FirstName());

            return bogusGenerator.Generate();
        }

        private static ResiliencyConfiguration GenerateResiliencyConfig()
        {
            var bogusGenerator = new Faker<ResiliencyConfiguration>()
                .StrictMode(ensureRulesForAllProperties: true)
                .RuleFor(eventTopic => eventTopic.Retry, faker => new RetryConfiguration
                {
                    Count = faker.UniqueIndex
                });

            return bogusGenerator.Generate();
        }
    }
}