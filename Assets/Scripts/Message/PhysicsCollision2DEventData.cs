using JetBrains.Annotations;
using UniFlow.Connector;
using UniFlow.Connector.Event;
using UnityEngine;

namespace UniFlow.Message
{
    [PublicAPI]
    public class PhysicsCollision2DEventData
    {
        private PhysicsCollision2DEventData(PhysicsCollision2DEventType eventType, Collision2D collision2D)
        {
            EventType = eventType;
            Collision2D = collision2D;
        }

        public PhysicsCollision2DEventType EventType { get; }
        public Collision2D Collision2D { get; }

        public static PhysicsCollision2DEventData Create(PhysicsCollision2DEventType eventType, Collision2D collision2D) =>
            new PhysicsCollision2DEventData(eventType, collision2D);
    }
}