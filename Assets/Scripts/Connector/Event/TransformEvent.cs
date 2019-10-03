using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/TransformEvent", (int) ConnectorType.TransformEvent)]
    public class TransformEvent : ConnectorBase
    {
        [SerializeField] private Component component = default;
        [SerializeField] private TransformEventType transformEventType = TransformEventType.TransformChildrenChanged;

        [ValuePublisher] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        [UsedImplicitly] public TransformEventType TransformEventType
        {
            get => transformEventType;
            set => transformEventType = value;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return OnEventAsObservable();
        }

        private IObservable<Unit> OnEventAsObservable()
        {
            switch (TransformEventType)
            {
                case TransformEventType.BeforeTransformParentChanged:
                    return Component.OnBeforeTransformParentChangedAsObservable();
                case TransformEventType.TransformParentChanged:
                    return Component.OnTransformParentChangedAsObservable();
                case TransformEventType.TransformChildrenChanged:
                    return Component.OnTransformChildrenChangedAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum TransformEventType
    {
        BeforeTransformParentChanged,
        TransformParentChanged,
        TransformChildrenChanged,
    }
}
