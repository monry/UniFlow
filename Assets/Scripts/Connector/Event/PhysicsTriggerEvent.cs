using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsTriggerEvent", (int) ConnectorType.PhysicsTriggerEvent)]
    public class PhysicsTriggerEvent : ConnectorBase
    {
        [SerializeField] private PhysicsTriggerEventType physicsTriggerEventType = PhysicsTriggerEventType.TriggerEnter;
        [SerializeField] private Component component = default;

        [UsedImplicitly] public PhysicsTriggerEventType PhysicsTriggerEventType
        {
            get => physicsTriggerEventType;
            set => physicsTriggerEventType = value;
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

        private IObservable<Collider> OnEventAsObservable()
        {
            switch (PhysicsTriggerEventType)
            {
                case PhysicsTriggerEventType.TriggerEnter:
                    return Component.OnTriggerEnterAsObservable();
                case PhysicsTriggerEventType.TriggerExit:
                    return Component.OnTriggerExitAsObservable();
                case PhysicsTriggerEventType.TriggerStay:
                    return Component.OnTriggerStayAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public class Message : MessageBase<PhysicsTriggerEvent, Collider>
        {
            public static Message Create(PhysicsTriggerEvent sender, Collider collider)
            {
                return Create<Message>(ConnectorType.PhysicsTriggerEvent, sender, collider);
            }
        }
    }

    public enum PhysicsTriggerEventType
    {
        TriggerEnter,
        TriggerExit,
        TriggerStay,
    }
}
