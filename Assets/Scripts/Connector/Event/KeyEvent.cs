using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/KeyEvent", (int) ConnectorType.KeyEvent)]
    public class KeyEvent : ConnectorBase
    {
        [SerializeField] private KeyEventType keyEventType = KeyEventType.Down;
        [SerializeField] private KeyCode keyCode = default;

        private KeyEventType KeyEventType => keyEventType;
        private KeyCode KeyCode => keyCode;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return KeyEventAsObservable().Select(this.CreateMessage);
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
