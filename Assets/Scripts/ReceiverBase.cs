using System;
using System.Collections.Generic;
using UniRx;

namespace UniFlow
{
    public abstract class ReceiverBase : ConnectorBase, IReceiver
    {
        protected override IEnumerable<IConnector> TargetConnectors { get; } = new List<IConnector>();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            OnReceive(Messages);
            return Observable.Empty<IMessage>();
        }

        public abstract void OnReceive(Messages messages);
    }
}