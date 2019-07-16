using System;
using EventConnector.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EventConnector.Connector
{
    public class RectTransformEvent : EventConnector
    {
        [SerializeField] private RectTransformEventType rectTransformEventType;
        private RectTransformEventType RectTransformEventType => rectTransformEventType;

        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component;
        private Component Component => component ? component : component = this;

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages)
        {
            return OnEventAsObservable().Select(_ => eventMessages.Append((Component, RectTransformEventData.Create(RectTransformEventType))));
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