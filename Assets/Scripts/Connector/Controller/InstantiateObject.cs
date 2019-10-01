using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/InstantiateObject", (int) ConnectorType.InstantiateObject)]
    public class InstantiateObject : ConnectorBase
    {
        [SerializeField] private Object source = default;

        [SerializeField] private Transform parent = default;

        [SerializeField] private PublishObjectEvent publisher = new PublishObjectEvent();
        [ValuePublisher("Instantiated")] private PublishObjectEvent Publisher => publisher;

        [ValueReceiver] public Object Source
        {
            get => source == default ? gameObject : source;
            set => source = value;
        }
        [ValueReceiver] public Transform Parent
        {
            get => parent == default ? transform : parent;
            set => parent = value;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            var go = Instantiate(Source, Parent);
            Publisher.Invoke(go);
            return Observable.ReturnUnit();
        }
    }
}
