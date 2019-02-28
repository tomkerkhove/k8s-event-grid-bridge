using System;
using k8s.Models;
using Newtonsoft.Json;

namespace Kubernetes.EventBridge.Sample
{
    public class EventPrinter : IEventHandler
    {
        public void HandleEvent(V1Event evt)
        {
            Console.WriteLine(JsonConvert.SerializeObject(evt));
        }
    }
}