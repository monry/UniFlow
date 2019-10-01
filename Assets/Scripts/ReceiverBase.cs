using System;
using System.Collections.Generic;
using UniRx;

namespace UniFlow
{
    public abstract class ReceiverBase : ConnectorBase, IReceiver
    {
        protected override IEnumerable<IConnector> TargetConnectors { get; } = new List<IConnector>();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            OnReceive();
            return Observable.ReturnUnit();
        }

        public abstract void OnReceive();
    }
}
