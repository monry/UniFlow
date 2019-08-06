using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EventConnector
{
    [PublicAPI]
    public struct EventMessages : IList<EventMessage>
    {
        private List<EventMessage> List { get; set; }
        private bool HasInitialized { get; set; }

        public EventMessages Append(EventMessage eventMessage)
        {
            InitializeIfNecessary();
            List.Add(eventMessage);
            return this;
        }

        public EventMessages AppendRange(IEnumerable<EventMessage> eventMessages)
        {
            InitializeIfNecessary();
            List.AddRange(eventMessages);
            return this;
        }

        public static EventMessages Create()
        {
            return new EventMessages();
        }

        private void InitializeIfNecessary()
        {
            if (HasInitialized)
            {
                return;
            }

            List = new List<EventMessage>();
            HasInitialized = true;
        }


        public IEnumerator<EventMessage> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(EventMessage item)
        {
            List.Add(item);
        }

        public void Clear()
        {
            List.Clear();
        }

        public bool Contains(EventMessage item)
        {
            return List.Contains(item);
        }

        public void CopyTo(EventMessage[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        public bool Remove(EventMessage item)
        {
            return List.Remove(item);
        }

        public int Count => List?.Count ?? 0;
        public bool IsReadOnly => true;

        public int IndexOf(EventMessage item)
        {
            return List.IndexOf(item);
        }

        public void Insert(int index, EventMessage item)
        {
            List.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            List.RemoveAt(index);
        }

        public EventMessage this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }
    }

    [PublicAPI]
    public struct EventMessage
    {
        private EventMessage(EventType eventType, object sender, object eventData)
        {
            EventType = eventType;
            Sender = sender;
            EventData = eventData;
        }

        public EventType EventType { get; }
        public object Sender { get; }
        public object EventData { get; }

        public static EventMessage Create(EventType eventType, object sender, object eventData = default)
        {
            return new EventMessage(eventType, sender, eventData);
        }
    }
}