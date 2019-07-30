using System;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
    public class Timer : EventConnector
    {
        [SerializeField] private float seconds = default;
        private float Seconds => seconds;

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages) =>
            Observable
                .Timer(TimeSpan.FromSeconds(Seconds))
                .Select(_ => eventMessages.Append((this, Seconds)))
                .FirstOrDefault();
    }
}