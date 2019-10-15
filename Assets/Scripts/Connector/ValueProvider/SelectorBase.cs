using System;
using System.Collections.Generic;
using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace UniFlow.Connector.ValueProvider
{
    public abstract class SelectorBase<TKey, TValue, TPublishEvent, TValueCollector> : ProviderBase<TValue, TPublishEvent, TValueCollector>, IValueProvider<TValue>
        where TPublishEvent : UnityEvent<TValue>, new()
        where TValueCollector : ValueCollectorBase<TValue>, new()
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        private IList<TKey> Keys => keys;
        private IList<TValue> Values => values;

        [ValueReceiver] public TKey Key { get; set; }

        TValue IValueProvider<TValue>.Provide()
        {
            if (!Keys.Contains(Key))
            {
                return default;
            }

            return Values[Keys.IndexOf(Key)];
        }
    }

    public abstract class GameObjectSelectorBase<TKey> : SelectorBase<TKey, GameObject, PublishGameObjectEvent, GameObjectCollector>
    {
    }

    public abstract class MonoBehaviourSelectorBase<TKey> : SelectorBase<TKey, MonoBehaviour, PublishMonoBehaviourEvent, MonoBehaviourCollector>
    {
    }

    public abstract class UIBehaviourSelectorBase<TKey> : SelectorBase<TKey, UIBehaviour, PublishUIBehaviourEvent, UIBehaviourCollector>
    {
    }

    public abstract class EnumSelectorBase<TKey, TEnum, TPublishEvent, TValueCollector> : SelectorBase<TKey, TEnum, TPublishEvent, TValueCollector>
        where TEnum : Enum
        where TPublishEvent : UnityEvent<TEnum>, new()
        where TValueCollector : ValueCollectorBase<TEnum>, new()
    {
    }
}
