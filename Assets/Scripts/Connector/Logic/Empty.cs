using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/Empty", (int) ConnectorType.Empty)]
    public class Empty : ConnectorBase
    {
        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage) =>
            Observable.Return(Message.Create(this));

        public class Message : MessageBase<Empty>
        {
            public static Message Create(Empty sender)
            {
                return Create<Message>(ConnectorType.Empty, sender);
            }
        }
    }
}