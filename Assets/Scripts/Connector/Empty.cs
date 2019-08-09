using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/Empty", 10000)]
    public class Empty : ConnectorBase
    {
        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Observable.Return(EventMessage.Create(ConnectorType.Empty, this));
    }
}