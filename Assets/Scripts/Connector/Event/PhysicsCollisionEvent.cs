using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsCollisionEvent", (int) ConnectorType.PhysicsCollisionEvent)]
    public class PhysicsCollisionEvent : ConnectorBase
    {
        [SerializeField] private PhysicsCollisionEventType physicsCollisionEventType = (PhysicsCollisionEventType) (-1);
        [SerializeField] private Component component = default;

        [UsedImplicitly] public PhysicsCollisionEventType PhysicsCollisionEventType
        {
            get => physicsCollisionEventType;
            set => physicsCollisionEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage) =>
            OnEventAsObservable()
                .Select(x => Message.Create(this, x));

        private IObservable<Collision> OnEventAsObservable()
        {
            switch (PhysicsCollisionEventType)
            {
                case PhysicsCollisionEventType.CollisionEnter:
                    return Component.OnCollisionEnterAsObservable();
                case PhysicsCollisionEventType.CollisionExit:
                    return Component.OnCollisionExitAsObservable();
                case PhysicsCollisionEventType.CollisionStay:
                    return Component.OnCollisionStayAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public class Message : MessageBase<PhysicsCollisionEvent, Collision>
        {
            public static Message Create(PhysicsCollisionEvent sender, Collision collision)
            {
                return Create<Message>(ConnectorType.PhysicsCollisionEvent, sender, collision);
            }
        }
    }

    public enum PhysicsCollisionEventType
    {
        CollisionEnter,
        CollisionExit,
        CollisionStay,
    }
}