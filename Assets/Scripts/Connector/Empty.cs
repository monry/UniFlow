using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/Empty", (int) ConnectorType.Empty)]
    public class Empty : ConnectorBase
    {
        public override IObservable<EventMessage> OnConnectAsObservable() =>
            Observable.Return(EventMessage.Create(ConnectorType.Empty, this));
    }
}