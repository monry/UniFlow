using System;
using System.Collections.Generic;
using UniFlow.Utility;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/CurrentRuntimePlatformProvider", (int) ConnectorType.ValueProviderCurrentRuntimePlatform)]
    public class CurrentRuntimePlatformProvider : ConnectorBase, IMessageComposable
    {
        private const string MessageParameterKey = "Value";

        private RuntimePlatform Value => Application.platform;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return ObservableFactory.ReturnMessage(this, MessageParameterKey, Value);
        }

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => Value, MessageParameterKey),
            };
    }
}
