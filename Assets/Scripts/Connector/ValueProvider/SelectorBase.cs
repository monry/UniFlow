using System;
using System.Collections.Generic;
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

        private IList<TKey> Keys => keys;
        private IList<TValue> Values => values;

        private TKey Key { get; set; }

        [SerializeField] private TKeyCollector keyCollector = new TKeyCollector();
        private TKeyCollector KeyCollector => keyCollector;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return Observable.Return(this.CreateMessage());
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<TKey>.Create(KeyCollector, x => Key = x, nameof(Key)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotation<TValue>.Create(() => Keys.Contains(Key) ? Values[Keys.IndexOf(Key)] : default),
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
