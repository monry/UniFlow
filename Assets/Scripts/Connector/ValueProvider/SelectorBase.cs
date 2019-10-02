using System;
using System.Collections.Generic;
using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace UniFlow.Connector.ValueProvider
{
    public abstract class SelectorBase<TKey, TValue, TPublishEvent> : ProviderBase<TValue, TPublishEvent> where TPublishEvent : UnityEvent<TValue>, new()
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

    public abstract class GameObjectSelectorBase<TKey> : SelectorBase<TKey, GameObject, PublishGameObjectEvent>
    {
    }

    public abstract class MonoBehaviourSelectorBase<TKey> : SelectorBase<TKey, MonoBehaviour, PublishMonoBehaviourEvent>
    {
    }

    public abstract class UIBehaviourSelectorBase<TKey> : SelectorBase<TKey, UIBehaviour, PublishUIBehaviourEvent>
    {
    }

    public abstract class EnumSelectorBase<TKey, TEnum, TPublishEvent> : SelectorBase<TKey, TEnum, TPublishEvent> where TEnum : Enum where TPublishEvent : UnityEvent<TEnum>, new()
    {
    }
}
