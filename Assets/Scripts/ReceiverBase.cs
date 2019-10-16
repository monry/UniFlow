using System;
using System.Collections.Generic;
using UniFlow.Utility;

namespace UniFlow
{
    public abstract class ReceiverBase : ConnectorBase, IReceiver
    {
        protected override IEnumerable<IConnector> TargetConnectors { get; } = new List<IConnector>();

        public override IObservable<Message> OnConnectAsObservable()
        {
            OnReceive();
            return ObservableFactory.ReturnMessage(this);
        }

        public abstract void OnReceive();
    }
}
