using System;
using System.Collections.Generic;
using UniFlow.Attribute;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/InstantiateObject", (int) ConnectorType.InstantiateGameObject)]
    public class InstantiateGameObject : ConnectorBase,
        IMessageCollectable,
        IMessageComposable
    {
        [SerializeField] private GameObject source = default;
        [SerializeField] private Transform parent = default;

        [ValueReceiver] public GameObject Source
        {
            get => source == default ? gameObject : source;
            set => source = value;
        }
        [ValueReceiver] public Transform Parent
        {
            get => parent == default ? transform : parent;
            set => parent = value;
        }

        [SerializeField] private GameObjectCollector sourceCollector = default;
        [SerializeField] private TransformCollector parentCollector = default;

        private GameObjectCollector SourceCollector => sourceCollector;
        private TransformCollector ParentCollector => parentCollector;

        private GameObject Instantiated { get; set; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            Instantiated = Instantiate(Source, Parent);
            return ObservableFactory.ReturnMessage(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<GameObject>(SourceCollector, x => Source = x, nameof(Source)),
                new CollectableMessageAnnotation<Transform>(ParentCollector, x => Parent = x, nameof(Parent)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                new ComposableMessageAnnotation<GameObject>(() => Instantiated, nameof(Instantiated)),
            };
    }
}
