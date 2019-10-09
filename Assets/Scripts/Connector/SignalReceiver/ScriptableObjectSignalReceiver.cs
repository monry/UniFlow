using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalReceiver
{
    [AddComponentMenu("UniFlow/SignalReceiver/ScriptableObject", (int) ConnectorType.ScriptableObjectSignalReceiver)]
    public class ScriptableObjectSignalReceiver : SignalReceiverBase<ScriptableObjectSignal>
    {
    }
}
