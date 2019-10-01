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
        [SerializeField] private RectTransformEventType rectTransformEventType = RectTransformEventType.CanvasGroupChanged;
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

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return OnEventAsObservable();
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
