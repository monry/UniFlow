using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/TransformSiblingController", (int) ConnectorType.TransformSiblingController)]
    public class TransformSiblingController : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private TransformSiblingControlMethod transformSiblingControlMethod = default;
        [SerializeField] private int index = default;

        private Transform TargetTransform
        {
            get => targetTransform != default
                ? targetTransform
                : targetTransform = transform;
            set => targetTransform = value;
        }
        private TransformSiblingControlMethod TransformSiblingControlMethod => transformSiblingControlMethod;
        private int Index
        {
            get => index;
            set => index = value;
        }

        [SerializeField] private TransformCollector targetTransformCollector = new TransformCollector();
        [SerializeField] private IntCollector indexCollector = new IntCollector();
        private TransformCollector TargetTransformCollector => targetTransformCollector;
        private IntCollector IndexCollector => indexCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            switch (TransformSiblingControlMethod)
            {
                case TransformSiblingControlMethod.SetIndex:
                    TargetTransform.SetSiblingIndex(Index);
                    break;
                case TransformSiblingControlMethod.AsFirst:
                    TargetTransform.SetAsFirstSibling();
                    break;
                case TransformSiblingControlMethod.AsLast:
                    TargetTransform.SetAsLastSibling();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return ObservableFactory.ReturnMessage(this);
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(TargetTransformCollector, x => TargetTransform = x, nameof(TargetTransform)),
                CollectableMessageAnnotationFactory.Create(IndexCollector, x => Index = x, nameof(Index)),
            };
    }

    public enum TransformSiblingControlMethod
    {
        SetIndex,
        AsFirst,
        AsLast,
    }
}
