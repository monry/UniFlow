using System;
using System.Collections.Generic;
using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace UniFlow.Connector.ValueProvider
{
    public abstract class SelectorBase<TKey, TValue> : ProviderBase<TValue>
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        private IList<TKey> Keys => keys;
        private IList<TValue> Values => values;

        [ValueReceiver] public TKey Key { get; set; }

        protected override TValue Provide()
        {
            if (!Keys.Contains(Key))
            {
                return default;
            }

            return Values[Keys.IndexOf(Key)];
        }
    }

    public abstract class GameObjectSelectorBase<TKey> : SelectorBase<TKey, GameObject>
    {
        [SerializeField] private PublishGameObjectEvent publisher = new PublishGameObjectEvent();
        [ValuePublisher("Value")] protected override UnityEvent<GameObject> Publisher => publisher;
    }

    public abstract class EnumSelectorBase<TKey, TEnum> : SelectorBase<TKey, TEnum> where TEnum : Enum
    {
    }
}
