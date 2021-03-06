using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/MoveParentTransform", (int) ConnectorType.MoveParentTransform)]
    public class MoveParentTransform : ConnectorBase,
        IMessageCollectable
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private Transform parentTransform = default;
        [SerializeField] private bool worldPositionStays = true;

        private Transform TargetTransform
        {
            get => targetTransform != default
                ? targetTransform
                : targetTransform = transform;
            set => targetTransform = value;
        }
        private Transform ParentTransform
        {
            get => parentTransform;
            set => parentTransform = value;
        }
        private bool WorldPositionStays
        {
            get => worldPositionStays;
            set => worldPositionStays = value;
        }

        [SerializeField] private TransformCollector targetTransformCollector = new TransformCollector();
        [SerializeField] private TransformCollector parentTransformCollector = new TransformCollector();
        [SerializeField] private BoolCollector worldPositionStaysCollector = new BoolCollector();

        private TransformCollector TargetTransformCollector => targetTransformCollector;
        private TransformCollector ParentTransformCollector => parentTransformCollector;
        private BoolCollector WorldPositionStaysCollector => worldPositionStaysCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            TargetTransform.SetParent(ParentTransform, WorldPositionStays);
            return ObservableFactory.ReturnMessage(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(TargetTransformCollector, x => TargetTransform = x, nameof(TargetTransform)),
                CollectableMessageAnnotationFactory.Create(ParentTransformCollector, x => ParentTransform = x, nameof(ParentTransform)),
                CollectableMessageAnnotationFactory.Create(WorldPositionStaysCollector, x => WorldPositionStays = x, nameof(WorldPositionStays)),
            };
    }
}
