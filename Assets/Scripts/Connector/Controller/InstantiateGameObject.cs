using System;
using System.Collections.Generic;
using System.Linq;
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

        protected GameObject Instantiated { get; set; }

        [Inject] private DiContainer DiContainer { get; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            Instantiated = DiContainer.InstantiatePrefab(Source, Parent);
            return ObservableFactory.ReturnMessage(this, nameof(Instantiated), Instantiated);
        }

        protected override IEnumerable<ICollectableMessageAnnotation> MergeMessageCollectableAnnotations() =>
            base.MergeMessageCollectableAnnotations()
                .Concat(
                    new[]
                    {
                        CollectableMessageAnnotationFactory.Create(SourceCollector, x => Source = x, nameof(Source)),
                        CollectableMessageAnnotationFactory.Create(ParentCollector, x => Parent = x, nameof(Parent)),
                    }
                );

        protected override IEnumerable<IComposableMessageAnnotation> MergeMessageComposableAnnotations() =>
            base.MergeMessageComposableAnnotations()
                .Concat(
                    new[]
                    {
                        ComposableMessageAnnotationFactory.Create<GameObject>(nameof(Instantiated)),
                    }
                );

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() => MergeMessageCollectableAnnotations();

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() => MergeMessageComposableAnnotations();
    }
}
