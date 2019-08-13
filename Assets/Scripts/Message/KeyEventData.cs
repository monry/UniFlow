using JetBrains.Annotations;
using UniFlow.Connector;
using UnityEngine;

namespace UniFlow.Message
{
    [PublicAPI]
    public class KeyEventData
    {
        private KeyEventData(KeyCode keyCode, KeyEventType keyEventType)
        {
            KeyCode = keyCode;
            KeyEventType = keyEventType;
        }

        public KeyCode KeyCode { get; }
        public KeyEventType KeyEventType { get; }

        public static KeyEventData Create(KeyCode keyCode, KeyEventType keyEventType)
        {
            return new KeyEventData(keyCode, keyEventType);
        }
    }
}