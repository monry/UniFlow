using JetBrains.Annotations;
using UniFlow.Connector;
using UnityEngine;

namespace UniFlow.Message
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