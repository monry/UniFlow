using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Logic
{
    [AddComponentMenu("UniFlow/Logic/BoolToggle", (int) ConnectorType.BoolToggle)]
    public class BoolToggle : ConnectorBase
    {
        [SerializeField] private bool initialValue = false;

        [UsedImplicitly] public bool InitialValue
        {
            get => initialValue;
            set => initialValue = value;
        }

        private bool Value { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Value = InitialValue;
        }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            var result = Observable.Return(Message.Create(this, Value));
            Value = !Value;

            return result;
        }

        public class Message : MessageBase<BoolToggle, bool>, IValueHolder<bool>
        {
            public static Message Create(BoolToggle sender, bool data)
            {
                return Create<Message>(ConnectorType.BoolToggle, sender, data);
            }

            bool IValueHolder<bool>.Value => Data;
        }
    }
}
