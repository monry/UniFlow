using System;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/KeyEvent", (int) ConnectorType.KeyEvent)]
    public class KeyEvent : ConnectorBase
    {
        [SerializeField] private KeyEventType keyEventType = (KeyEventType) (-1);
        [SerializeField] private KeyCode keyCode = default;

        private KeyEventType KeyEventType => keyEventType;
        private KeyCode KeyCode => keyCode;

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return KeyEventAsObservable().Select(_ => EventMessage.Create(ConnectorType.KeyEvent, this, KeyEventData.Create(KeyCode, KeyEventType)));
        }

        private IObservable<Unit> KeyEventAsObservable()
        {
            switch (KeyEventType)
            {
                case KeyEventType.Down:
                    return this
                        .UpdateAsObservable()
                        .Where(_ => Input.GetKeyDown(KeyCode))
                        .AsUnitObservable();
                case KeyEventType.Press:
                    return this
                        .UpdateAsObservable()
                        .Where(_ => Input.GetKey(KeyCode))
                        .AsUnitObservable();
                case KeyEventType.Up:
                    return this
                        .UpdateAsObservable()
                        .Where(_ => Input.GetKeyUp(KeyCode))
                        .AsUnitObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum KeyEventType
    {
        Down,
        Press,
        Up,
    }
}