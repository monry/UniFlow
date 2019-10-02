using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/ScriptableObject", (int) ConnectorType.ValueProviderScriptableObject)]
    public class ScriptableObjectProvider : ProviderBase<ScriptableObject, PublishScriptableObjectEvent>
    {
        [SerializeField] private ScriptableObject value = default;
        private ScriptableObject Value => value;

        protected override ScriptableObject Provide()
        {
            return Value;
        }
    }
}
