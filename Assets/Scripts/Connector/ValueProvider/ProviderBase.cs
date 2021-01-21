using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    public abstract class ProviderBase<TValue> : ConnectorBase, IInjectable<TValue>, IMessageComposable
    {
        private const string MessageParameterKey = "Value";

        [SerializeField] private TValue value = default;
        public TValue Value
        {
            get => value;
            set => this.value = value;
        }

        public override IObservable<Message> OnConnectAsObservable()
        {
            return ObservableFactory.ReturnMessage(this, MessageParameterKey, Value);
        }

        void IInjectable<TValue>.Inject(TValue v)
        {
            Value = v;
        }

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => Value, MessageParameterKey),
            };
    }
}
