using System;

namespace UniFlow
{
    public class ComposableMessageAnnotation
    {
        public ComposableMessageAnnotation(IConnector connector, Type type) : this(connector, type, type.Name)
        {
        }

        public ComposableMessageAnnotation(IConnector connector, Type type, string key)
        {
            Type = type;
            Connector = connector;
            Key = key;
        }

        public Type Type { get; }
        public IConnector Connector { get; }
        public string Key { get; }
        public string Label => string.IsNullOrEmpty(Key) ? Type.Name : Key;
    }

    public class CollectableMessageAnnotation
    {
        public CollectableMessageAnnotation(Type type, IValueCollector valueCollector) : this(type, valueCollector, type.Name)
        {
        }

        public CollectableMessageAnnotation(Type type, IValueCollector valueCollector, string label)
        {
            Type = type;
            ValueCollector = valueCollector;
            Label = label;
        }

        public Type Type { get; }
        public IValueCollector ValueCollector { get; }
        public string Label { get; }
    }
}
