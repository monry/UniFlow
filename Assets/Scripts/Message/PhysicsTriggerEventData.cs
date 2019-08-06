using EventConnector.Connector;
using JetBrains.Annotations;
using UnityEngine;

namespace EventConnector.Message
{
    [PublicAPI]
    public class PhysicsTriggerEventData
    {
        private PhysicsTriggerEventData(PhysicsTriggerEventType eventType, Collider collider)
        {
            EventType = eventType;
            Collider = collider;
        }

        public PhysicsTriggerEventType EventType { get; }
        public Collider Collider { get; }

        public static PhysicsTriggerEventData Create(PhysicsTriggerEventType eventType, Collider collider) =>
            new PhysicsTriggerEventData(eventType, collider);
    }
}