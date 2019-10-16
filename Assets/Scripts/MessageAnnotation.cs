using System;
using UnityEngine;

namespace UniFlow
{
    public interface IComposableMessageAnnotation
    {
        Type Type { get; }
        string Key { get; }
        Message Compose(Message message);
    }

    public class ComposableMessageAnnotation<TValue> : IComposableMessageAnnotation
    {
        public ComposableMessageAnnotation(Func<TValue> callback) : this(callback, typeof(TValue).Name)
        {
        }

        public ComposableMessageAnnotation(Func<TValue> callback, string key)
        {
            Type = typeof(TValue);
            Callback = callback;
            Key = key;
        }

        public Type Type { get; }
        public string Key { get; }
        private Func<TValue> Callback { get; }

        Message IComposableMessageAnnotation.Compose(Message message)
        {
            if (Callback != default)
            {
                message = message.AddParameter(Callback(), Key);
            }

            return message;
        }
    }

    public interface ICollectableMessageAnnotation
    {
        Type Type { get; }
        IValueCollector ValueCollector { get; }
        string Key { get; }
        void Collect();
        void Inject(GameObject gameObject);
    }

    public class CollectableMessageAnnotation<TValue> : ICollectableMessageAnnotation
    {
        public CollectableMessageAnnotation(IValueCollector valueCollector, Action<TValue> callback) : this(valueCollector, callback, typeof(TValue).Name)
        {
        }

        public CollectableMessageAnnotation(IValueCollector valueCollector, Action<TValue> callback, string key)
        {
            Type = typeof(TValue);
            ValueCollector = valueCollector;
            Callback = callback;
            Key = key;
        }

        public Type Type { get; }
        public IValueCollector ValueCollector { get; }
        public string Key { get; }
        private Action<TValue> Callback { get; }

        void ICollectableMessageAnnotation.Collect()
        {
            if (ValueCollector?.TargetConnector != default && ValueCollector?.SourceConnector != default)
            {
                Callback?.Invoke(GetValue());
            }
        }

        void ICollectableMessageAnnotation.Inject(GameObject gameObject)
        {
            gameObject.GetComponentsInChildren<IInjectable<TValue>>().InjectAll(GetValue());
        }

        private TValue GetValue()
        {
            var result = default(TValue);
            if (ValueCollector is IValueCollector<TValue> valueCollector && valueCollector.CanCollect())
            {
                result = valueCollector.Collect();
            }

            return result;
        }
    }
}
