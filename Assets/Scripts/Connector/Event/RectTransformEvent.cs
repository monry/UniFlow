using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/RectTransformEvent", (int) ConnectorType.RectTransformEvent)]
    public class RectTransformEvent : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private Component component = default;
        [SerializeField] private RectTransformEventType rectTransformEventType = RectTransformEventType.CanvasGroupChanged;

        private Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        private RectTransformEventType RectTransformEventType => rectTransformEventType;

        [SerializeField] private ComponentCollector componentCollector = new ComponentCollector();

        private ComponentCollector ComponentCollector => componentCollector;

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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<Component>.Create(ComponentCollector, x => Component = x),
            };
    }

    public enum RectTransformEventType
    {
        CanvasGroupChanged,
        RectTransformDimensionsChange,
        RectTransformRemoved,
     }
 }
