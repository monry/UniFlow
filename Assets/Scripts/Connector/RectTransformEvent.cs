using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/RectTransformEvent", 103)]
    public class RectTransformEvent : ConnectorBase
    {
        [SerializeField] private RectTransformEventType rectTransformEventType = default;
        private RectTransformEventType RectTransformEventType
        {
            get => rectTransformEventType;
            [UsedImplicitly]
            set => rectTransformEventType = value;
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
                .Select(_ => EventMessage.Create(ConnectorType.RectTransformEvent, Component, RectTransformEventData.Create(RectTransformEventType)));
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