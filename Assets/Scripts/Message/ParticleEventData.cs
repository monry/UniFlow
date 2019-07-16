using EventConnector.Connector;
using JetBrains.Annotations;
using UnityEngine;

namespace EventConnector.Message
{
    [PublicAPI]
    public class ParticleEventData
    {
        private ParticleEventData(ParticleEventType eventType, GameObject gameObject)
        {
            EventType = eventType;
            GameObject = gameObject;
        }

        public ParticleEventType EventType { get; }
        public GameObject GameObject { get; }

        public static ParticleEventData Create(ParticleEventType eventType, GameObject collision) =>
            new ParticleEventData(eventType, collision);
    }
}