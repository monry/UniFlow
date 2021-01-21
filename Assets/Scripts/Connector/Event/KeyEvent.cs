using System;
using System.Collections.Generic;
using KeyEventHandler;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/KeyEvent", (int) ConnectorType.KeyEvent)]
    public class KeyEvent : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private KeyCode keyCode = default;
        [SerializeField] private KeyEventType keyEventType = default;

        private KeyCode KeyCode
        {
            get => keyCode;
            set => keyCode = value;
        }
        private KeyEventType KeyEventType
        {
            get => keyEventType;
            set => keyEventType = value;
        }

        [SerializeField] private KeyCodeCollector keyCodeCollector = new KeyCodeCollector();
        [SerializeField] private KeyEventTypeCollector keyEventTypeCollector = new KeyEventTypeCollector();

        private KeyCodeCollector KeyCodeCollector => keyCodeCollector;
        private KeyEventTypeCollector KeyEventTypeCollector => keyEventTypeCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            switch (KeyEventType)
            {
                case KeyEventType.Down:
                    return this.OnKeyDownAsObservable(KeyCode).AsMessageObservable(this);
                case KeyEventType.Press:
                    return this.OnKeyPressAsObservable(KeyCode).AsMessageObservable(this);
                case KeyEventType.Up:
                    return this.OnKeyUpAsObservable(KeyCode).AsMessageObservable(this);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(KeyCodeCollector, x => KeyCode = x, nameof(KeyCode)),
                CollectableMessageAnnotationFactory.Create(KeyEventTypeCollector, x => KeyEventType = x, nameof(KeyEventType)),
            };
    }

    public enum KeyEventType
    {
        Down,
        Press,
        Up,
    }

    [Serializable]
    public class KeyCodeCollector : ValueCollectorBase<KeyCode>
    {
    }

    [Serializable]
    public class KeyEventTypeCollector : ValueCollectorBase<KeyEventType>
    {
    }
}
