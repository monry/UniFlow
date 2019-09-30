using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/InstantiateObject", (int) ConnectorType.InstantiateObject)]
    public class InstantiateObject : ConnectorBase, IValueProvider<Object>
    {
        [SerializeField] private Object source = default;

        [SerializeField] private Transform parent = default;

        [SerializeField] [ValuePublisher("Instantiated", ValueInjectionType.Object)] private PublishObjectEvent publishInstantiatedGameObject = default;
        private PublishObjectEvent PublishInstantiatedGameObject => publishInstantiatedGameObject ?? (publishInstantiatedGameObject = new PublishObjectEvent());

        [SerializeField] [ValuePublisher("Vecror2", ValueInjectionType.Vector2)] private PublishObjectEvent publishVector2 = default;
        private PublishObjectEvent PublishVector2 => publishVector2 ?? (publishVector2 = new PublishObjectEvent());

        [SerializeField] [ValuePublisher("Int", ValueInjectionType.Int)] private PublishIntEvent publishInt = default;
        private PublishIntEvent PublishInt => publishInt ?? (publishInt = new PublishIntEvent());

        [UsedImplicitly] public Object Source
        {
            get => source == default ? gameObject : source;
            set => source = value;
        }
        [UsedImplicitly] public Transform Parent
        {
            get => parent == default ? transform : parent;
            set => parent = value;
        }

        private ISubject<Object> ObjectSubject { get; } = new Subject<Object>();
        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            if (latestMessage.IsValueHolder<Object>())
            {
                Source = latestMessage.GetHeldValue<Object>();
            }

            var go = Instantiate(Source, Parent);
            PublishInstantiatedGameObject.Invoke(go as GameObject);
            ObjectSubject.OnNext(go);
            return Observable.Return(Message.Create(this, go));
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }

        IObservable<Object> IValueProvider<Object>.OnProvideAsObservable()
        {
            return ObjectSubject;
        }

        public class Message : MessageBase<InstantiateObject, Object>, IValueHolder<Object>
        {
            Object IValueHolder<Object>.Value => Data;

            public static Message Create(InstantiateObject connectable, Object obj)
            {
                return Create<Message>(ConnectorType.InstantiateObject, connectable, obj);
            }
        }
    }
}
