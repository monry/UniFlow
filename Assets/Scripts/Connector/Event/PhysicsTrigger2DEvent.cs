using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsTrigger2DEvent", (int) ConnectorType.PhysicsTrigger2DEvent)]
    public class PhysicsTrigger2DEvent : ConnectorBase
    {
        [SerializeField] private PhysicsTrigger2DEventType physicsTrigger2DEventType = PhysicsTrigger2DEventType.TriggerEnter2D;
        [SerializeField] private Component component = default;

        [UsedImplicitly] public PhysicsTrigger2DEventType PhysicsTrigger2DEventType
        {
            get => physicsTrigger2DEventType;
            set => physicsTrigger2DEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return OnEventAsObservable()
                .Select(x => Message.Create(this, x));
        }

        private IObservable<Collider2D> OnEventAsObservable()
        {
            switch (PhysicsTrigger2DEventType)
            {
                case PhysicsTrigger2DEventType.TriggerEnter2D:
                    return Component.OnTriggerEnter2DAsObservable();
                case PhysicsTrigger2DEventType.TriggerExit2D:
                    return Component.OnTriggerExit2DAsObservable();
                case PhysicsTrigger2DEventType.TriggerStay2D:
                    return Component.OnTriggerStay2DAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public class Message : MessageBase<PhysicsTrigger2DEvent, Collider2D>
        {
            public static Message Create(PhysicsTrigger2DEvent sender, Collider2D collider2D)
            {
                return Create<Message>(ConnectorType.PhysicsTrigger2DEvent, sender, collider2D);
            }
        }
    }

    public enum PhysicsTrigger2DEventType
    {
        TriggerEnter2D,
        TriggerExit2D,
        TriggerStay2D,
    }
}