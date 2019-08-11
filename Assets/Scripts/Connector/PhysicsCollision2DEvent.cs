using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/PhyticsCollision2DEvent", 201)]
    public class PhysicsCollision2DEvent : ConnectorBase
    {
        [SerializeField] private PhysicsCollision2DEventType physicsCollision2DEventType = default;
        [UsedImplicitly] public PhysicsCollision2DEventType PhysicsCollision2DEventType
        {
            get => physicsCollision2DEventType;
            set => physicsCollision2DEventType = value;
        }

        private Component component = default;
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable() =>
            OnEventAsObservable()
                .Select(x => EventMessage.Create(ConnectorType.PhysicsCollision2DEvent, Component, PhysicsCollision2DEventData.Create(PhysicsCollision2DEventType, x)));

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
    }

    public enum PhysicsCollision2DEventType
    {
        CollisionEnter2D,
        CollisionExit2D,
        CollisionStay2D,
    }
}