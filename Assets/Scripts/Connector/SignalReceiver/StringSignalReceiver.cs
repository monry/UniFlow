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
        IMessageComposable
    {
        [SerializeField] private string signalName = default;
        [ValueReceiver] public string SignalName
        {
            get => signalName;
            set => signalName = value;
        }

        [SerializeField] private PublishBoolEvent publishBoolParameter = new PublishBoolEvent();
        [SerializeField] private PublishIntEvent publishIntParameter = new PublishIntEvent();
        [SerializeField] private PublishFloatEvent publishFloatParameter = new PublishFloatEvent();
        [SerializeField] private PublishStringEvent publishStringParameter = new PublishStringEvent();
        [SerializeField] private PublishObjectEvent publishObjectParameter = new PublishObjectEvent();
        [SerializeField] private PublishScriptableObjectEvent publishScriptableObjectParameter = new PublishScriptableObjectEvent();

        [ValuePublisher("BoolParameter")] private PublishBoolEvent PublishBoolParameter => publishBoolParameter;
        [ValuePublisher("IntParameter")] private PublishIntEvent PublishIntParameter => publishIntParameter;
        [ValuePublisher("FloatParameter")] private PublishFloatEvent PublishFloatParameter => publishFloatParameter;
        [ValuePublisher("StringParameter")] private PublishStringEvent PublishStringParameter => publishStringParameter;
        [ValuePublisher("ObjectParameter")] private PublishObjectEvent PublishObjectParameter => publishObjectParameter;
        [ValuePublisher("ScriptableObjectParameter")] private PublishScriptableObjectEvent PublishScriptableObjectParameter => publishScriptableObjectParameter;

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
            PublishBoolParameter.Invoke(receivedSignal.Parameter.BoolValue);
            PublishIntParameter.Invoke(receivedSignal.Parameter.IntValue);
            PublishFloatParameter.Invoke(receivedSignal.Parameter.FloatValue);
            PublishStringParameter.Invoke(receivedSignal.Parameter.StringValue);
            PublishObjectParameter.Invoke(receivedSignal.Parameter.ObjectValue);
            PublishScriptableObjectParameter.Invoke(receivedSignal.Parameter.ScriptableObjectValue);

            Signal = receivedSignal;
        }

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
