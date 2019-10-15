using System;
using System.Collections.Generic;
using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace UniFlow.Connector.ValueProvider
{
    public abstract class SelectorBase<TKey, TValue, TPublishEvent, TKeyCollector> : ProviderBase<TValue, TPublishEvent>, IValueProvider<TValue>, IMessageCollectable
        where TPublishEvent : UnityEvent<TValue>, new()
        where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        private IList<TKey> Keys => keys;
        private IList<TValue> Values => values;

        [ValueReceiver] public TKey Key { get; set; }

        [SerializeField] private TKeyCollector keyCollector = default;
        private TKeyCollector KeyCollector => keyCollector ?? (keyCollector = new TKeyCollector());

        TValue IValueProvider<TValue>.Provide()
        {
            if (!Keys.Contains(Key))
            {
                return default;
            }

            return Values[Keys.IndexOf(Key)];
        }

        public override IEnumerable<ComposableMessageAnnotation> GetMessageComposableAnnotations() =>
            new[]
            {
                new ComposableMessageAnnotation(this, typeof(TValue)),
            };

        public override Message Compose(Message message)
        {
            return message
                .AddParameter(Keys.Contains(Key) ? Values[Keys.IndexOf(Key)] : default);
        }

        public IEnumerable<CollectableMessageAnnotation> GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation(typeof(TKey), KeyCollector),
            };

        public virtual void Collect()
        {
            KeyCollector.Collect(StreamedMessages);
        }

        public virtual void RegisterCollectDelegates()
        {
            KeyCollector.Action = x => Key = x;
        }

    }

    public abstract class GameObjectSelectorBase<TKey, TKeyCollector> : SelectorBase<TKey, GameObject, PublishGameObjectEvent, TKeyCollector> where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
    }

    public abstract class MonoBehaviourSelectorBase<TKey, TKeyCollector> : SelectorBase<TKey, MonoBehaviour, PublishMonoBehaviourEvent, TKeyCollector> where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
    }

    public abstract class UIBehaviourSelectorBase<TKey, TKeyCollector> : SelectorBase<TKey, UIBehaviour, PublishUIBehaviourEvent, TKeyCollector> where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
    }

    public abstract class EnumSelectorBase<TKey, TEnum, TPublishEvent, TKeyCollector> : SelectorBase<TKey, TEnum, TPublishEvent, TKeyCollector>
        where TEnum : Enum
        where TPublishEvent : UnityEvent<TEnum>, new()
        where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
    }
}
