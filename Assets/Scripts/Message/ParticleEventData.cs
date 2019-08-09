using JetBrains.Annotations;
using UniFlow.Connector;
using UnityEngine;

namespace UniFlow.Message
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

        public static ParticleEventData Create(ParticleEventType eventType, GameObject gameObject) =>
            new ParticleEventData(eventType, gameObject);
    }
}