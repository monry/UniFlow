using UniFlow.Attribute;
using UniFlow.Connector.SignalPublisher;
using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalReceiver
{
    [AddComponentMenu("UniFlow/SignalReceiver/String", (int) ConnectorType.StringSignalReceiver)]
    public class StringSignalReceiver : SignalReceiverBase<StringSignal>, ISignalCreator<StringSignal>, ISignalFilter<StringSignal>
    {
        [SerializeField] private string signalName = default;
        [ValueReceiver] public string SignalName
        {
            get => signalName;
            set => signalName = value;
        }

        bool ISignalFilter<StringSignal>.Filter(StringSignal signal)
        {
            return signal.Name == SignalName;
        }

        StringSignal ISignalCreator<StringSignal>.CreateSignal()
        {
            return string.IsNullOrEmpty(SignalName) ? Signal : new StringSignal(SignalName);
        }
    }
}
