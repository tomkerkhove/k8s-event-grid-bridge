namespace eventrouter {
    using System;
    using k8s.Models;

    interface IEventHandler
    {
        void HandleEvent(V1Event obj);
    }
}