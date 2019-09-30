using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3Int", (int) ConnectorType.ValueProviderVector3Int)]
    public class Vector3Int : Base<UnityEngine.Vector3Int>
    {
        [SerializeField] private UnityEngine.Vector3Int value = default;
        private UnityEngine.Vector3Int Value => value;

        [SerializeField] private PublishObjectEvent publisher = default;
        [ValuePublisher("Value", ValueInjectionType.Vector3Int)]
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Vector3Int Provide()
        {
            var publishValue = ScriptableObject.CreateInstance<Vector3IntObject>();
            publishValue.Value = Value;
            Publisher.Invoke(publishValue);
            return Value;
        }
    }
}
