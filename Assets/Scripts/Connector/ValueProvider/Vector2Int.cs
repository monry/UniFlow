using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2Int", (int) ConnectorType.ValueProviderVector2Int)]
    public class Vector2Int : Base<UnityEngine.Vector2Int>
    {
        [SerializeField] private UnityEngine.Vector2Int value = default;
        private UnityEngine.Vector2Int Value => value;

        [SerializeField] private PublishObjectEvent publisher = default;
        [ValuePublisher("Value", ValueInjectionType.Vector2Int)]
        private PublishObjectEvent Publisher => publisher ?? (publisher = new PublishObjectEvent());

        protected override UnityEngine.Vector2Int Provide()
        {
            var publishValue = ScriptableObject.CreateInstance<Vector2IntObject>();
            publishValue.Value = Value;
            Publisher.Invoke(publishValue);
            return Value;
        }
    }
}
