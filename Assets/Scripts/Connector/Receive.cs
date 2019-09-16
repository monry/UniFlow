using System;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/Misc/Receive", (int) ConnectorType.Receive)]
    public class Receive : ConnectorBase
    {
        // force return false to prevent triggering
        public override bool ActAsTrigger
        {
            get => false;
            set
            {
            }
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return Observable
                .Create<IMessage>(
                    observer =>
                    {
                        observer.OnNext(Message.Create(this));
                        return Disposable;
                    }
                );
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }

        public class Message : MessageBase<Receive>
        {
            public static Message Create(Receive sender)
            {
                return Create<Message>(ConnectorType.Receive, sender);
            }
        }
    }
}