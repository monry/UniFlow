using System;
using System.Collections.Generic;
using System.Linq;
using UniFlow.Utility;
using UnityEngine;
using Zenject;

namespace UniFlow.Connector.Controller
{
    public abstract class InstantiateGameObjectWithInjectionBase : ConnectorBase,
        IMessageCollectable,
        IMessageComposable
    {
        [SerializeField] private GameObject source = default;
        [SerializeField] private Transform parent = default;

        private GameObject Source
        {
            get => source == default ? gameObject : source;
            set => source = value;
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
            GetMessageCollectableAnnotations().ToList().ForEach(x => x.Inject(Instantiated));
            return ObservableFactory.ReturnMessage(this);
        }

        protected virtual IEnumerable<ICollectableMessageAnnotation> GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(SourceCollector, x => Source = x, nameof(Source)),
                CollectableMessageAnnotationFactory.Create(ParentCollector, x => Parent = x, nameof(Parent)),
            };

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            GetMessageCollectableAnnotations();

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => Instantiated),
            };
    }
}
