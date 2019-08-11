using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/TransformEvent", 102)]
    public class TransformEvent : ConnectorBase
    {
        [SerializeField] private TransformEventType transformEventType = default;
        private TransformEventType TransformEventType
        {
            get => transformEventType;
            [UsedImplicitly]
            set => transformEventType = value;
        }

        private Component component = default;
        private Component Component
        {
            get => component ? component : component = this;
            [UsedImplicitly]
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