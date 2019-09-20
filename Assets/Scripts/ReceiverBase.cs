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
            return Observable.Return(Message.Create(this));
        }

        public abstract void OnReceive(Messages messages);

        public class Message : MessageBase<ReceiverBase>
        {
            public static Message Create(ReceiverBase sender)
            {
                return Create<Message>(ConnectorType.Receiver, sender);
            }
        }
    }
}