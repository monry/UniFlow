using System;
using JetBrains.Annotations;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsCollision2DEvent", (int) ConnectorType.PhysicsCollision2DEvent)]
    public class PhysicsCollision2DEvent : ConnectorBase
    {
        [SerializeField] private PhysicsCollision2DEventType physicsCollision2DEventType = PhysicsCollision2DEventType.CollisionEnter2D;
        [SerializeField] private Component component = default;

        [UsedImplicitly] public PhysicsCollision2DEventType PhysicsCollision2DEventType
        {
            get => physicsCollision2DEventType;
            set => physicsCollision2DEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage) =>
            OnEventAsObservable()
                .Select(x => Message.Create(this, x));

        private IObservable<Collision2D> OnEventAsObservable()
        {
            switch (PhysicsCollision2DEventType)
            {
                case PhysicsCollision2DEventType.CollisionEnter2D:
                    return Component.OnCollisionEnter2DAsObservable();
                case PhysicsCollision2DEventType.CollisionExit2D:
                    return Component.OnCollisionExit2DAsObservable();
                case PhysicsCollision2DEventType.CollisionStay2D:
                    return Component.OnCollisionStay2DAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public class Message : MessageBase<PhysicsCollision2DEvent, Collision2D>
        {
            public static Message Create(PhysicsCollision2DEvent sender, Collision2D collision2D)
            {
                return Create<Message>(ConnectorType.PhysicsCollision2DEvent, sender, collision2D);
            }
        }
    }

    public enum PhysicsCollision2DEventType
    {
        CollisionEnter2D,
        CollisionExit2D,
        CollisionStay2D,
    }
}