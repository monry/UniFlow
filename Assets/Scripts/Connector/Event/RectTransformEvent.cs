using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/RectTransformEvent", (int) ConnectorType.RectTransformEvent)]
    public class RectTransformEvent : ConnectorBase
    {
        [SerializeField] private Component component = default;
        [SerializeField] private RectTransformEventType rectTransformEventType = RectTransformEventType.CanvasGroupChanged;

        [ValuePublisher] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        [UsedImplicitly] public RectTransformEventType RectTransformEventType
        {
            get => rectTransformEventType;
            set => rectTransformEventType = value;
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnEventAsObservable().Select(this.CreateMessage);
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
    }

    public enum RectTransformEventType
    {
        CanvasGroupChanged,
        RectTransformDimensionsChange,
        RectTransformRemoved,
     }
 }
