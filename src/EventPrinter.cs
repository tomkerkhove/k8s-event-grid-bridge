namespace eventrouter {
    using System;

    using k8s.Models;
    using Newtonsoft.Json;

    public class EventPrinter : IEventHandler {
        public void HandleEvent(V1Event evt) {
            Console.WriteLine(JsonConvert.SerializeObject(evt));
        }
    }
}