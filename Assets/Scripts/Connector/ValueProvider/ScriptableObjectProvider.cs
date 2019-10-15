using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/ScriptableObject", (int) ConnectorType.ValueProviderScriptableObject)]
    public class ScriptableObjectProvider : ProviderBase<ScriptableObject, PublishScriptableObjectEvent, ScriptableObjectCollector>
    {
    }
}
