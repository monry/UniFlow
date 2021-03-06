using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace UniFlow.Connector.ValueProvider
{
    public abstract class SelectorBase<TKey, TValue, TKeyCollector> : ConnectorBase,
        IMessageCollectable,
        IMessageComposable
        where TKeyCollector : ValueCollectorBase<TKey>, new()
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        protected IList<TKey> Keys => keys != default && keys.Any() ? keys : keys = GetKeys().ToList();
        protected IEnumerable<TValue> Values => values != default && values.Any() ? values : values = GetValues().ToList();

        private TKey Key { get; set; }

        [SerializeField] private TKeyCollector keyCollector = new TKeyCollector();
        private TKeyCollector KeyCollector => keyCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Observable.Return(this.CreateMessage());
        }

        protected virtual IEnumerable<TKey> GetKeys()
        {
            return keys;
        }

        protected virtual IEnumerable<TValue> GetValues()
        {
            return values;
        }

        protected virtual TValue FindValue(TKey key)
        {
            return Keys.Contains(key) && Values.Count() > Keys.IndexOf(key) ? Values.ElementAt(Keys.IndexOf(key)) : default;
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(KeyCollector, x => Key = x, nameof(Key)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => FindValue(Key)),
            };
    }

    public abstract class ListSelectorBase<TValue> : SelectorBase<int, TValue, IntCollector>
    {
        protected override IEnumerable<int> GetKeys()
        {
            return Enumerable.Range(0, Values.Count());
        }
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
