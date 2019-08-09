using System;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/TransformEvent", 102)]
    public class TransformEvent : EventPublisher
    {
        [SerializeField] private TransformEventType transformEventType = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private TransformEventType TransformEventType => transformEventType;
        private Component Component => component ? component : component = this;

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            OnEventAsObservable()
                .Select(_ => EventMessage.Create(EventType.TransformEvent, Component, TransformEventData.Create(TransformEventType)));

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