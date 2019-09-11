using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/RectTransformEvent", (int) ConnectorType.RectTransformEvent)]
    public class RectTransformEvent : ConnectorBase
    {
        [SerializeField] private RectTransformEventType rectTransformEventType = (RectTransformEventType) (-1);
        [SerializeField] private Component component = default;

        [UsedImplicitly] public RectTransformEventType RectTransformEventType
        {
            get => rectTransformEventType;
            set => rectTransformEventType = value;
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
            switch (RectTransformEventType)
            {
                case RectTransformEventType.CanvasGroupChanged:
                    return Component.OnCanvasGroupChangedAsObservable();
                case RectTransformEventType.RectTransformDimensionsChange:
                    return Component.OnRectTransformDimensionsChangeAsObservable();
                case RectTransformEventType.RectTransformRemoved:
                    return Component.OnRectTransformRemovedAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public class Message : MessageBase<RectTransformEvent>
        {
            public static Message Create(RectTransformEvent sender)
            {
                return Create<Message>(ConnectorType.RectTransformEvent, sender);
            }
        }
    }

    public enum RectTransformEventType
    {
        CanvasGroupChanged,
        RectTransformDimensionsChange,
        RectTransformRemoved,
     }
 }