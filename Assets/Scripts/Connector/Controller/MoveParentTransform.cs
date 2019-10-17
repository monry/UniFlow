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

        [SerializeField] private TransformCollector targetTransformCollector = default;
        [SerializeField] private TransformCollector parentTransformCollector = default;
        [SerializeField] private BoolCollector worldPositionStaysCollector = default;

        private TransformCollector TargetTransformCollector => targetTransformCollector;
        private TransformCollector ParentTransformCollector => parentTransformCollector;
        private BoolCollector WorldPositionStaysCollector => worldPositionStaysCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            TargetTransform.SetParent(ParentTransform, WorldPositionStays);
            return ObservableFactory.ReturnMessage(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                CollectableMessageAnnotation<Transform>.Create(TargetTransformCollector, x => TargetTransform = x, nameof(TargetTransform)),
                CollectableMessageAnnotation<Transform>.Create(ParentTransformCollector, x => ParentTransform = x, nameof(ParentTransform)),
                CollectableMessageAnnotation<bool>.Create(WorldPositionStaysCollector, x => WorldPositionStays = x, nameof(WorldPositionStays)),
            };
    }
}
