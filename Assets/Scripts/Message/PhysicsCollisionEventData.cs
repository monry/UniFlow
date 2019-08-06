using EventConnector.Connector;
using JetBrains.Annotations;
using UnityEngine;

namespace EventConnector.Message
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