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

        [SerializeField] private StringCollector signalNameCollector = new StringCollector();

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
                CollectableMessageAnnotationFactory.Create(SignalNameCollector, x => SignalName = x, nameof(SignalName)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => Signal.Parameter.BoolValue, "BoolParameter"),
                ComposableMessageAnnotationFactory.Create(() => Signal.Parameter.IntValue, "IntParameter"),
                ComposableMessageAnnotationFactory.Create(() => Signal.Parameter.FloatValue, "FloatParameter"),
                ComposableMessageAnnotationFactory.Create(() => Signal.Parameter.StringValue, "StringParameter"),
                ComposableMessageAnnotationFactory.Create(() => Signal.Parameter.ObjectValue, "ObjectParameter"),
                ComposableMessageAnnotationFactory.Create(() => Signal.Parameter.ScriptableObjectValue, "ScriptableObjectParameter"),
            };
    }
}
