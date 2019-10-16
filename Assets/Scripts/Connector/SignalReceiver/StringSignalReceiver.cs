using System.Collections.Generic;
using UniFlow.Attribute;
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
        [ValueReceiver] public string SignalName
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
                new CollectableMessageAnnotation<string>(SignalNameCollector, x => SignalName = x, nameof(SignalName)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new IComposableMessageAnnotation[]
            {
                new ComposableMessageAnnotation<bool>(() => Signal.Parameter.BoolValue, "BoolParameter"),
                new ComposableMessageAnnotation<int>(() => Signal.Parameter.IntValue, "IntParameter"),
                new ComposableMessageAnnotation<float>(() => Signal.Parameter.FloatValue, "FloatParameter"),
                new ComposableMessageAnnotation<string>(() => Signal.Parameter.StringValue, "StringParameter"),
                new ComposableMessageAnnotation<Object>(() => Signal.Parameter.ObjectValue, "ObjectParameter"),
                new ComposableMessageAnnotation<ScriptableObject>(() => Signal.Parameter.ScriptableObjectValue, "ScriptableObjectParameter"),
            };
    }
}
