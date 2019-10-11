using UniFlow.Attribute;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalPublisher
{
    [AddComponentMenu("UniFlow/SignalPublisher/String", (int) ConnectorType.StringSignalPublisher)]
    public class StringSignalPublisher : SignalPublisherBase<StringSignal>, ISignalCreator<StringSignal>
    {
        [SerializeField] private string signalName = default;
        [ValueReceiver] public string SignalName
        {
            get => signalName;
            set => signalName = value;
        }

        StringSignal ISignalCreator<StringSignal>.CreateSignal()
        {
            return string.IsNullOrEmpty(SignalName) ? Signal : new StringSignal(SignalName);
        }
    }
}
