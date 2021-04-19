using System.IO;

namespace Kubernetes.EventGrid.Tests.Unit.Events
{
    public static class KubernetesEventSamples
    {
        public static string GetRawContainerStartedEvent()
        {
            return ReadEventFromDisk("Core", "ContainerStarted.json");
        }

        public static string GetRawClusterAutoscalerScaleOutEvent()
        {
            return ReadEventFromDisk("Cluster-Autoscaler", "TriggeredScaleUp.json");
        }

        public static string GetRawReplicaSetScaleInEvent()
        {
            return ReadEventFromDisk("Core", "ScalingReplicaSetDown.json");
        }

        public static string GetRawReplicaSetScaleOutEvent()
        {
            return ReadEventFromDisk("Core", "ScalingReplicaSetUp.json");
        }

        private static string ReadEventFromDisk(string folderName, string fileName)
        {
            var filePath = Path.Combine("Events", "Samples", folderName, fileName);
            return File.ReadAllText(filePath);
        }
    }
}
