using System;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
    public class Timer : EventConnector
    {
        [SerializeField] private float seconds = default;
        private float Seconds => seconds;

        protected override void Connect(EventMessages eventMessages) =>
            Observable
                .Timer(TimeSpan.FromSeconds(Seconds))
                .SubscribeWithState(eventMessages, (_, em) => OnConnect(em.Append(EventMessage.Create(EventType.Timer, this, Seconds))));
    }
}