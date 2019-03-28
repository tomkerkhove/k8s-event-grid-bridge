using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Kubernetes.EventBridge.Host.Serialization
{
    public class YamlSerialization
    {
        private static readonly INamingConvention namingConvention = new CamelCaseNamingConvention();

        public static IDeserializer CreateDeserializer() => new DeserializerBuilder().WithNamingConvention(namingConvention).Build();

        public static ISerializer CreateSerializer() => new SerializerBuilder().WithNamingConvention(namingConvention).Build();
    }
}