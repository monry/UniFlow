using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/ScriptableObjectProvider", (int) ConnectorType.ValueProviderScriptableObject)]
    public class ScriptableObjectProvider : ProviderBase<ScriptableObject>
    {
    }
}
