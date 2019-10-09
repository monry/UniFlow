using UniFlow.Signal;
using UnityEngine;

namespace UniFlow.Connector.SignalPublisher
{
    [AddComponentMenu("UniFlow/SignalPublisher/ScriptableObject", (int) ConnectorType.ScriptableObjectSignalPublisher)]
    public class ScriptableObjectSignalPublisher : SignalPublisherBase<ScriptableObjectSignal>
    {
    }
}
