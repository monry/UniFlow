using System;
using JetBrains.Annotations;
using UniFlow.Connector.Event;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/KeyInputController", (int) ConnectorType.KeyInputController)]
    public class KeyInputController : ConnectorBase
    {
        [SerializeField] private KeyInputControlMethod keyInputControlMethod = KeyInputControlMethod.Activate;

        [UsedImplicitly] private KeyInputControlMethod KeyInputControlMethod
        {
            get => keyInputControlMethod;
            set => keyInputControlMethod = value;
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            HandleActivation();
            return ObservableFactory.ReturnMessage(this);
        }

        private void HandleActivation()
        {
            switch (KeyInputControlMethod)
            {
                case KeyInputControlMethod.Activate:
                    KeyEvent.Gate.Value = true;
                    break;
                case KeyInputControlMethod.Deactivate:
                    KeyEvent.Gate.Value = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    [PublicAPI]
    public enum KeyInputControlMethod
    {
        Activate,
        Deactivate,
    }
}
