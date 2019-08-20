using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/TransformEvent", (int) ConnectorType.TransformEvent)]
    public class TransformEvent : ConnectorBase
    {
        [SerializeField] private TransformEventType transformEventType = default;
        [SerializeField] private Component component = default;

        [UsedImplicitly] public TransformEventType TransformEventType
        {
            get => transformEventType;
            set => transformEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(_ => EventMessage.Create(ConnectorType.TransformEvent, Component, TransformEventData.Create(TransformEventType)));
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