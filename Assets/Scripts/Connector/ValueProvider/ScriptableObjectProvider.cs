using UniFlow.Attribute;
using UnityEngine;
using UnityEngine.Events;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/ScriptableObject", (int) ConnectorType.ValueProviderScriptableObject)]
    public class ScriptableObjectProvider : ProviderBase<ScriptableObject>
    {
        [SerializeField] private ScriptableObject value = default;
        private ScriptableObject Value => value;

        [SerializeField] private PublishScriptableObjectEvent publisher = new PublishScriptableObjectEvent();
        [ValuePublisher("Value")] protected override UnityEvent<ScriptableObject> Publisher => publisher;

        protected override ScriptableObject Provide()
        {
            return Value;
        }
    }
}
