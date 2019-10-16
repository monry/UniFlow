using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsCollision2DEvent", (int) ConnectorType.PhysicsCollision2DEvent)]
    public class PhysicsCollision2DEvent : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private Component component = default;
        [SerializeField] private PhysicsCollision2DEventType physicsCollision2DEventType = PhysicsCollision2DEventType.CollisionEnter2D;

        private Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        private PhysicsCollision2DEventType PhysicsCollision2DEventType => physicsCollision2DEventType;

        [SerializeField] private ComponentCollector componentCollector = default;

        private ComponentCollector ComponentCollector => componentCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(this.CreateMessage);
        }

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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation<Component>(ComponentCollector, x => Component = x),
            };
    }

    public enum PhysicsCollision2DEventType
    {
        CollisionEnter2D,
        CollisionExit2D,
        CollisionStay2D,
    }
}
