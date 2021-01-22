using System.IO;

namespace Kubernetes.EventGrid.Tests.Unit.Events
{
    public static class KubernetesEventSamples
    {
        public static string GetRawContainerStartedEvent()
        {
            return ReadEventFromDisk("ContainerStarted.json");
        }

        private static string ReadEventFromDisk(string fileName)
        {
            return File.ReadAllText($"Events\\Samples\\{fileName}");
        }
    }
}
