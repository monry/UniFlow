using System.Collections.Generic;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalReceiver
{
    [AddComponentMenu("UniFlow/SignalReceiver/StringSignalReceiver", (int) ConnectorType.StringSignalReceiver)]
    public class StringSignalReceiver : SignalReceiverBase<StringSignal, StringSignalCollector>,
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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(SignalNameCollector, x => SignalName = x, nameof(SignalName)),
            };

        IEnumerable<IComposableMessageAnnotation> IMessageComposable.GetMessageComposableAnnotations() =>
            new[]
            {
                ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Parameter.BoolValue, "BoolParameter"),
                ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Parameter.IntValue, "IntParameter"),
                ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Parameter.FloatValue, "FloatParameter"),
                ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Parameter.StringValue, "StringParameter"),
                ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Parameter.ObjectValue, "ObjectParameter"),
                ComposableMessageAnnotationFactory.Create(() => ReceivedSignal.Parameter.ScriptableObjectValue, "ScriptableObjectParameter"),
            };

        protected override StringSignal GetSignal()
        {
            return StringSignal.Create(SignalName);
        }
    }
}
