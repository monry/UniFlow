using System.Collections.Generic;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalPublisher
{
    [AddComponentMenu("UniFlow/SignalPublisher/String", (int) ConnectorType.StringSignalPublisher)]
    public class StringSignalPublisher : SignalPublisherBase<StringSignal, StringSignalCollector>,
        IMessageCollectable
    {
        [SerializeField] private string signalName = default;
        [SerializeField] private bool boolParameter = default;
        [SerializeField] private int intParameter = default;
        [SerializeField] private float floatParameter = default;
        [SerializeField] private string stringParameter = default;
        [SerializeField] private Object objectParameter = default;
        [SerializeField] private ScriptableObject scriptableObjectParameter = default;

        private string SignalName
        {
            get => signalName;
            set => signalName = value;
        }
        private bool BoolParameter
        {
            get => boolParameter;
            set => boolParameter = value;
        }
        private int IntParameter
        {
            get => intParameter;
            set => intParameter = value;
        }
        private float FloatParameter
        {
            get => floatParameter;
            set => floatParameter = value;
        }
        private string StringParameter
        {
            get => stringParameter;
            set => stringParameter = value;
        }
        private Object ObjectParameter
        {
            get => objectParameter;
            set => objectParameter = value;
        }
        private ScriptableObject ScriptableObjectParameter
        {
            get => scriptableObjectParameter;
            set => scriptableObjectParameter = value;
        }

        [SerializeField] private StringCollector signalNameCollector = new StringCollector();
        [SerializeField] private BoolCollector boolCollector = new BoolCollector();
        [SerializeField] private IntCollector intCollector = new IntCollector();
        [SerializeField] private FloatCollector floatCollector = new FloatCollector();
        [SerializeField] private StringCollector stringCollector = new StringCollector();
        [SerializeField] private ObjectCollector objectCollector = new ObjectCollector();
        [SerializeField] private ScriptableObjectCollector scriptableObjectCollector = new ScriptableObjectCollector();

        private StringCollector SignalNameCollector => signalNameCollector;
        private BoolCollector BoolCollector => boolCollector;
        private IntCollector IntCollector => intCollector;
        private FloatCollector FloatCollector => floatCollector;
        private StringCollector StringCollector => stringCollector;
        private ObjectCollector ObjectCollector => objectCollector;
        private ScriptableObjectCollector ScriptableObjectCollector => scriptableObjectCollector;

        protected override StringSignal GetSignal()
        {
            return StringSignal
                .Create(
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

        IEnumerable<ICollectableMessageAnnotation> IMessageCollectable.GetMessageCollectableAnnotations() =>
            new[]
            {
                CollectableMessageAnnotationFactory.Create(SignalNameCollector, x => SignalName = x, nameof(SignalName)),
                CollectableMessageAnnotationFactory.Create(BoolCollector, x => BoolParameter = x, nameof(BoolParameter)),
                CollectableMessageAnnotationFactory.Create(IntCollector, x => IntParameter = x, nameof(IntParameter)),
                CollectableMessageAnnotationFactory.Create(FloatCollector, x => FloatParameter = x, nameof(FloatParameter)),
                CollectableMessageAnnotationFactory.Create(StringCollector, x => StringParameter = x, nameof(StringParameter)),
                CollectableMessageAnnotationFactory.Create(ObjectCollector, x => ObjectParameter = x, nameof(ObjectParameter)),
                CollectableMessageAnnotationFactory.Create(ScriptableObjectCollector, x => ScriptableObjectParameter = x, nameof(ScriptableObjectParameter)),
            };
    }
}
