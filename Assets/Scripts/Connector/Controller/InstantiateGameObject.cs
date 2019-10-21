using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;
using Zenject;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/InstantiateGameObject", (int) ConnectorType.InstantiateGameObject)]
    public class InstantiateGameObject : ConnectorBase,
        IMessageCollectable,
        IMessageComposable
    {
        [SerializeField] private GameObject source = default;
        [SerializeField] private Transform parent = default;

        public GameObject Source
        {
            get => source == default ? gameObject : source;
            private set => source = value;
        }
        private Transform Parent
        {
            get => parent == default ? transform : parent;
            set => parent = value;
        }

        [SerializeField] private GameObjectCollector sourceCollector = new GameObjectCollector();
        [SerializeField] private TransformCollector parentCollector = new TransformCollector();

        private GameObjectCollector SourceCollector => sourceCollector;
        private TransformCollector ParentCollector => parentCollector;

        private GameObject Instantiated { get; set; }

        [Inject] private DiContainer DiContainer { get; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            Instantiated = DiContainer.InstantiatePrefab(Source, Parent);
            return ObservableFactory.ReturnMessage(this, nameof(Instantiated), Instantiated);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(SourceCollector, x => Source = x, nameof(Source)),
                CollectableMessageAnnotationFactory.Create(ParentCollector, x => Parent = x, nameof(Parent)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create<GameObject>(nameof(Instantiated)),
            };
    }
}
