using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/MouseEvent", (int) ConnectorType.MouseEvent)]
    public class MouseEvent : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private Component component = default;
        [SerializeField] private MouseEventType mouseEventType = MouseEventType.MouseDown;

        private Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        private MouseEventType MouseEventType => mouseEventType;

        [SerializeField] private ComponentCollector componentCollector = new ComponentCollector();

        private ComponentCollector ComponentCollector => componentCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnEventAsObservable().Select(this.CreateMessage);
        }

        private IObservable<Unit> OnEventAsObservable()
        {
#if !(UNITY_IOS || UNITY_ANDROID || UNITY_METRO)
            switch (MouseEventType)
            {
                case MouseEventType.MouseDown:
                    return Component.OnMouseDownAsObservable();
                case MouseEventType.MouseUp:
                    return Component.OnMouseUpAsObservable();
                case MouseEventType.MouseUpAsButton:
                    return Component.OnMouseUpAsButtonAsObservable();
                case MouseEventType.MouseEnter:
                    return Component.OnMouseEnterAsObservable();
                case MouseEventType.MouseExit:
                    return Component.OnMouseExitAsObservable();
                case MouseEventType.MouseOver:
                    return Component.OnMouseOverAsObservable();
                case MouseEventType.MouseDrag:
                    return Component.OnMouseDragAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
#else
            throw new PlatformNotSupportedException("MouseEvent does not support mobile platform");
#endif
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(ComponentCollector, x => Component = x),
            };
    }

    public enum MouseEventType
    {
        MouseDown,
        MouseUp,
        MouseUpAsButton,
        MouseEnter,
        MouseExit,
        MouseOver,
        MouseDrag,
    }
}
