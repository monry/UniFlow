using System;
using JetBrains.Annotations;
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

        [SerializeField] private PublishObjectEvent publishInstantiatedGameObject = default;
        [ValuePublisher("Instantiated")]
        private PublishObjectEvent PublishInstantiatedGameObject => publishInstantiatedGameObject ?? (publishInstantiatedGameObject = new PublishObjectEvent());

        [SerializeField] private PublishVector2Event publishVector2 = default;
        [ValuePublisher]
        private PublishVector2Event PublishVector2 => publishVector2 ?? (publishVector2 = new PublishVector2Event());

        [SerializeField] private PublishIntEvent publishInt = default;
        [ValuePublisher]
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

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            var go = Instantiate(Source, Parent);
            PublishInstantiatedGameObject.Invoke(go as GameObject);
            return Observable.ReturnUnit();
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}