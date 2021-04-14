﻿using System;
using Kubernetes.EventGrid.Bridge.Contracts.Enums;

namespace Kubernetes.EventGrid.Core.Kubernetes.Events.Interfaces
{
    public interface IKubernetesEvent
    {
        public KubernetesEventType Type { get; }
        public object Payload { get; }
        public Uri? Source { get; }
        public string Subject { get; }

        /// <summary>
        ///     Namespace from which it was emitted
        /// </summary>
        public string Namespace { get; set; }
    }
}