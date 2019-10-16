using System.Collections.Generic;
using UniFlow.Attribute;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalPublisher
{
    [AddComponentMenu("UniFlow/SignalPublisher/String", (int) ConnectorType.StringSignalPublisher)]
    public class StringSignalPublisher : SignalPublisherBase<StringSignal>,
        ISignalCreator<StringSignal>,
        IMessageCollectable
    {
        [SerializeField] private string signalName = default;
        [SerializeField] private bool boolParameter = default;
        [SerializeField] private int intParameter = default;
        [SerializeField] private float floatParameter = default;
        [SerializeField] private string stringParameter = default;
        [SerializeField] private Object objectParameter = default;
        [SerializeField] private ScriptableObject scriptableObjectParameter = default;

        [ValueReceiver] public string SignalName
        {
            get => signalName;
            set => signalName = value;
        }
        [ValueReceiver] public bool BoolParameter
        {
            get => boolParameter;
            set => boolParameter = value;
        }
        [ValueReceiver] public int IntParameter
        {
            get => intParameter;
            set => intParameter = value;
        }
        [ValueReceiver] public float FloatParameter
        {
            get => floatParameter;
            set => floatParameter = value;
        }
        [ValueReceiver] public string StringParameter
        {
            get => stringParameter;
            set => stringParameter = value;
        }
        [ValueReceiver] public Object ObjectParameter
        {
            get => objectParameter;
            set => objectParameter = value;
        }
        [ValueReceiver] public ScriptableObject ScriptableObjectParameter
        {
            get => scriptableObjectParameter;
            set => scriptableObjectParameter = value;
        }

        StringSignal ISignalCreator<StringSignal>.CreateSignal()
        {
            return string.IsNullOrEmpty(SignalName)
                ? Signal
                : new StringSignal(
                    SignalName,
                    new StringSignal.SignalParameter(
                        BoolParameter,
                        IntParameter,
                        FloatParameter,
                        StringParameter,
                        ObjectParameter,
                        ScriptableObjectParameter
                    )
                );
        }

        [SerializeField] private BoolCollector boolCollector = default;
        [SerializeField] private IntCollector intCollector = default;
        [SerializeField] private FloatCollector floatCollector = default;
        [SerializeField] private StringCollector stringCollector = default;
        [SerializeField] private ObjectCollector objectCollector = default;
        [SerializeField] private ScriptableObjectCollector scriptableObjectCollector = default;

        private BoolCollector BoolCollector => boolCollector;
        private IntCollector IntCollector => intCollector;
        private FloatCollector FloatCollector => floatCollector;
        private StringCollector StringCollector => stringCollector;
        private ObjectCollector ObjectCollector => objectCollector;
        private ScriptableObjectCollector ScriptableObjectCollector => scriptableObjectCollector;

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new ICollectableMessageAnnotation[]
            {
                new CollectableMessageAnnotation<bool>(BoolCollector, x => BoolParameter = x, nameof(BoolParameter)),
                new CollectableMessageAnnotation<int>(IntCollector, x => IntParameter = x, nameof(IntParameter)),
                new CollectableMessageAnnotation<float>(FloatCollector, x => FloatParameter = x, nameof(FloatParameter)),
                new CollectableMessageAnnotation<string>(StringCollector, x => StringParameter = x, nameof(StringParameter)),
                new CollectableMessageAnnotation<Object>(ObjectCollector, x => ObjectParameter = x, nameof(ObjectParameter)),
                new CollectableMessageAnnotation<ScriptableObject>(ScriptableObjectCollector, x => ScriptableObjectParameter = x, nameof(ScriptableObjectParameter)),
            };
    }
}
