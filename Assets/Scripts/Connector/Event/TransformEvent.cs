using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/TransformEvent", (int) ConnectorType.TransformEvent)]
    public class TransformEvent : ConnectorBase
    {
        [SerializeField] private TransformEventType transformEventType = TransformEventType.TransformChildrenChanged;
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

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return OnEventAsObservable()
                .Select(_ => Message.Create(this));
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

        public class Message : MessageBase<TransformEvent>
        {
            public static Message Create(TransformEvent sender)
            {
                return Create<Message>(ConnectorType.TransformEvent, sender);
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