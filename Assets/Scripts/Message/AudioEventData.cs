using UniFlow.Connector;
using UniFlow.Connector.Event;

namespace UniFlow.Message
{
    public struct AudioEventData
    {
        private AudioEventData(AudioEventType audioEventType)
        {
            EventType = audioEventType;
        }

        public AudioEventType EventType { get; }

        public static AudioEventData Create(AudioEventType audioEventType) =>
            new AudioEventData(audioEventType);
    }
}