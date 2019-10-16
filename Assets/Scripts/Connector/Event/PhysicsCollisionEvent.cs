using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/PhysicsCollisionEvent", (int) ConnectorType.PhysicsCollisionEvent)]
    public class PhysicsCollisionEvent : ConnectorBase, IMessageCollectable
    {
        [SerializeField] private Component component = default;
        [SerializeField] private PhysicsCollisionEventType physicsCollisionEventType = PhysicsCollisionEventType.CollisionEnter;

        private Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        private PhysicsCollisionEventType PhysicsCollisionEventType => physicsCollisionEventType;

        [SerializeField] private ComponentCollector componentCollector = default;

        private ComponentCollector ComponentCollector => componentCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(this.CreateMessage);
        }

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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation<Component>(ComponentCollector, x => Component = x),
            };
    }

    public enum PhysicsCollisionEventType
    {
        CollisionEnter,
        CollisionExit,
        CollisionStay,
    }
}
