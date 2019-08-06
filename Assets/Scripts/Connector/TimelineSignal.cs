using EventConnector.Message;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
    // Timeline SignalReceiver cannot serialize [Serializable] class
    // So I provide overloads to construct TimelineEventData
    [AddComponentMenu("Event Connector/TimelineSignal")]
    public class TimelineSignal : EventConnector
    {
        private ISubject<TimelineEventData> Subject { get; set; } = new Subject<TimelineEventData>();
        private EventMessages EventMessages { get; set; } = EventMessages.Create();

        protected override void Connect(EventMessages eventMessages) =>
            EventMessages = eventMessages;

        public void Dispatch()
        {
            OnConnect(EventMessages.Append(EventMessage.Create(EventType.TimelineSignal, this, new TimelineEventData())));
        }

        public void Dispatch(float floatParameter)
        {
            OnConnect(EventMessages.Append(EventMessage.Create(EventType.TimelineSignal, this, new TimelineEventData(floatParameter))));
        }

        public void Dispatch(int intParameter)
        {
            OnConnect(EventMessages.Append(EventMessage.Create(EventType.TimelineSignal, this, new TimelineEventData(intParameter))));
        }

        public void Dispatch(string stringParameter)
        {
            OnConnect(EventMessages.Append(EventMessage.Create(EventType.TimelineSignal, this, new TimelineEventData(stringParameter))));
        }

        public void Dispatch(Object objectReferenceParameter)
        {
            OnConnect(EventMessages.Append(EventMessage.Create(EventType.TimelineSignal, this, new TimelineEventData(objectReferenceParameter))));
        }
    }
}