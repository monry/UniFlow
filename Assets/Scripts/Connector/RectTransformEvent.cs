using System;
using EventConnector.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EventConnector.Connector
{
    public class RectTransformEvent : EventConnector
    {
        [SerializeField] private RectTransformEventType rectTransformEventType = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private RectTransformEventType RectTransformEventType => rectTransformEventType;
        private Component Component => component ? component : component = this;

        protected override void Connect(EventMessages eventMessages)
        {
            OnEventAsObservable()
                .SubscribeWithState(
                    eventMessages,
                    (_, em) => em.Append(EventMessage.Create(EventType.RectTransformEvent, Component, RectTransformEventData.Create(RectTransformEventType)))
                );
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