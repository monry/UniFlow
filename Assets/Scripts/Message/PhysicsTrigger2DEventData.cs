using JetBrains.Annotations;
using UniFlow.Connector;
using UnityEngine;

namespace UniFlow.Message
{
    [PublicAPI]
    public class PhysicsTrigger2DEventData
    {
        private PhysicsTrigger2DEventData(PhysicsTrigger2DEventType eventType, Collider2D collider2D)
        {
            EventType = eventType;
            Collider2D = collider2D;
        }

        public PhysicsTrigger2DEventType EventType { get; }
        public Collider2D Collider2D { get; }

        public static PhysicsTrigger2DEventData Create(PhysicsTrigger2DEventType eventType, Collider2D collider2D) =>
            new PhysicsTrigger2DEventData(eventType, collider2D);
    }
}