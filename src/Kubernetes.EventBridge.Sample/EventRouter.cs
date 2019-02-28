using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Kubernetes.EventBridge.Sample.Handlers;

namespace Kubernetes.EventBridge.Sample
{
    internal class EventRouter
    {
        private static async Task Main(string[] args)
        {
            var _namespace = "sandbox";
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new k8s.Kubernetes(config);
            Console.WriteLine("Starting Watcher!");

            var handlers = new List<IEventHandler>();
            //handlers.Add(new EventPrinter());
            handlers.Add(new CloudEventPrinter());

            while (true)
            {
                // TODO: this is imperfect, I think it will double-send events.
                try
                {
                    var eventlistResp = await client.ListNamespacedEventWithHttpMessagesAsync(_namespace, watch: true);
                    using (eventlistResp.Watch<V1Event>((type, item) =>
                    {
                        foreach (var handler in handlers)
                        {
                            handler.HandleEvent(item);
                        }
                    }))
                    {
                        Console.WriteLine("press ctrl + c to stop watching");

                        var ctrlc = new ManualResetEventSlim(false);
                        Console.CancelKeyPress += (sender, eventArgs) => ctrlc.Set();
                        ctrlc.Wait();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}