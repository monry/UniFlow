using System.Collections.Generic;
using UniFlow.Connector.SignalPublisher;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalReceiver
{
    [AddComponentMenu("UniFlow/SignalReceiver/String", (int) ConnectorType.StringSignalReceiver)]
    public class StringSignalReceiver : SignalReceiverBase<StringSignal>,
        ISignalCreator<StringSignal>,
        ISignalFilter<StringSignal>,
        IMessageCollectable,
        IMessageComposable
    {
        [SerializeField] private string signalName = default;

        private string SignalName
        {
            get => signalName;
            set => signalName = value;
        }

        [SerializeField] private StringCollector signalNameCollector = default;

        private StringCollector SignalNameCollector => signalNameCollector;

        bool ISignalFilter<StringSignal>.Filter(StringSignal signal)
        {
            return signal.Name == SignalName;
        }

        StringSignal ISignalCreator<StringSignal>.CreateSignal()
        {
            return string.IsNullOrEmpty(SignalName) ? Signal : new StringSignal(SignalName);
        }

        protected override void OnReceive(StringSignal receivedSignal)
        {
            Signal = receivedSignal;
        }

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotation<string>.Create(SignalNameCollector, x => SignalName = x, nameof(SignalName)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                ComposableMessageAnnotation<bool>.Create(() => Signal.Parameter.BoolValue, "BoolParameter"),
                ComposableMessageAnnotation<int>.Create(() => Signal.Parameter.IntValue, "IntParameter"),
                ComposableMessageAnnotation<float>.Create(() => Signal.Parameter.FloatValue, "FloatParameter"),
                ComposableMessageAnnotation<string>.Create(() => Signal.Parameter.StringValue, "StringParameter"),
                ComposableMessageAnnotation<Object>.Create(() => Signal.Parameter.ObjectValue, "ObjectParameter"),
                ComposableMessageAnnotation<ScriptableObject>.Create(() => Signal.Parameter.ScriptableObjectValue, "ScriptableObjectParameter"),
            };
    }
}
