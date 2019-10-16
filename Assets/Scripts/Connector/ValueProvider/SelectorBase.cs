using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace UniFlow.Connector.ValueProvider
{
    public abstract class SelectorBase<TKey, TValue, TKeyCollector> : ProviderBase<TValue>,
        IMessageCollectable,
        IMessageComposable
        where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        private IList<TKey> Keys => keys;
        private IList<TValue> Values => values;

        public TKey Key { get; set; }

        [SerializeField] private TKeyCollector keyCollector = default;
        private TKeyCollector KeyCollector => keyCollector;

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                new ComposableMessageAnnotation<TValue>(() => Keys.Contains(Key) ? Values[Keys.IndexOf(Key)] : default),
            };

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                new CollectableMessageAnnotation<TKey>(KeyCollector, x => Key = x),
            };
    }

    public abstract class GameObjectSelectorBase<TKey, TKeyCollector> : SelectorBase<TKey, GameObject, TKeyCollector> where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
    }

    public abstract class MonoBehaviourSelectorBase<TKey, TKeyCollector> : SelectorBase<TKey, MonoBehaviour, TKeyCollector> where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
    }

    public abstract class UIBehaviourSelectorBase<TKey, TKeyCollector> : SelectorBase<TKey, UIBehaviour, TKeyCollector> where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
    }

    public abstract class EnumSelectorBase<TKey, TEnum, TKeyCollector> : SelectorBase<TKey, TEnum, TKeyCollector>
        where TEnum : Enum
        where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
    }
}
