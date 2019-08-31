using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/MouseEvent", (int) ConnectorType.MouseEvent)]
    public class MouseEvent : ConnectorBase
    {
        [SerializeField] private MouseEventType mouseEventType = (MouseEventType) (-1);
        [SerializeField] private Component component = default;

        [UsedImplicitly] public MouseEventType MouseEventType
        {
            get => mouseEventType;
            set => mouseEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable() =>
            OnEventAsObservable()
                .Select(_ => EventMessage.Create(ConnectorType.MouseEvent, Component, MouseEventData.Create(MouseEventType)));

        private IObservable<Unit> OnEventAsObservable()
        {
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
        }
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