using System;
using UniFlow.Connector.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    public class CreateInstance : ConnectorBase
    {
        [SerializeField] private GameObject source = default;
        private GameObject Source => source == default ? gameObject : source;

        [SerializeField] private Transform parent = default;
        private Transform Parent => parent == default ? transform : parent;

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return Observable
                .Create<IMessage>(
                    observer =>
                    {
                        var go = Instantiate(Source, Parent);
                        observer.OnNext(Message.Create(this, go));
                        return Disposable;
                    }
                );
        }

        public class Message : MessageBase<CreateInstance, GameObject>, IValueHolder<GameObject>
        {
            GameObject IValueHolder<GameObject>.Value => Data;

            public static Message Create(CreateInstance connectable, GameObject gameObject)
            {
                return Create<Message>(ConnectorType.Custom, connectable, gameObject);
            }
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}