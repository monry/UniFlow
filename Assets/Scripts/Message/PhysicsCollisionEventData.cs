using JetBrains.Annotations;
using UniFlow.Connector;
using UnityEngine;

namespace UniFlow.Message
{
    [PublicAPI]
    public class PhysicsCollisionEventData
    {
        private PhysicsCollisionEventData(PhysicsCollisionEventType eventType, Collision collision)
        {
            EventType = eventType;
            Collision = collision;
        }

        public PhysicsCollisionEventType EventType { get; }
        public Collision Collision { get; }

        public static PhysicsCollisionEventData Create(PhysicsCollisionEventType eventType, Collision collision) =>
            new PhysicsCollisionEventData(eventType, collision);
    }
}