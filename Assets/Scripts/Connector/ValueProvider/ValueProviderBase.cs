using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ValueProviderBase<TValue, TPublishEvent> : ProviderBase<TValue, TPublishEvent> where TPublishEvent : UnityEvent<TValue>, new()
    {
        [SerializeField] private TValue value = default;
        [UsedImplicitly] public TValue Value
        {
            get => value;
            set => this.value = value;
        }
    }
}
