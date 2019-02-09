using System;
using System.Collections.Generic;
using System.Threading;

using k8s;
using k8s.Models;
using Newtonsoft.Json;

namespace eventrouter
{
    internal class EventRouter
    {
        private static void Main(string[] args)
        {
            var _namespace = "default";
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
            IKubernetes client = new Kubernetes(config);
            Console.WriteLine("Starting Watcher!");

            var handlers = new List<IEventHandler>();
            handlers.Add(new EventPrinter());

            while (true)
            {
                // TODO: this is imperfect, I think it will double-send events.
                try
                {
                    var eventlistResp = client.ListNamespacedEventWithHttpMessagesAsync(_namespace, watch: true).Result;
                    using (eventlistResp.Watch<V1Event>((type, item) =>
                    {
                        foreach(var handler in handlers) {
                            handler.HandleEvent(item);
                        }
                    }))
                    {
                        Console.WriteLine("press ctrl + c to stop watching");

                        var ctrlc = new ManualResetEventSlim(false);
                        Console.CancelKeyPress += (sender, eventArgs) => ctrlc.Set();
                        ctrlc.Wait();
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
