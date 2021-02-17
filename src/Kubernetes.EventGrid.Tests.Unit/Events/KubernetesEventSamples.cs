﻿using System.IO;

namespace Kubernetes.EventGrid.Tests.Unit.Events
{
    public static class KubernetesEventSamples
    {
        public static string GetRawContainerStartedEvent()
        {
            return ReadEventFromDisk("Core", "ContainerStarted.json");
        }

        private static string ReadEventFromDisk(string folderName, string fileName)
        {
            var filePath = Path.Combine("Events", "Samples", folderName, fileName);
            return File.ReadAllText(filePath);
        }
    }
}
