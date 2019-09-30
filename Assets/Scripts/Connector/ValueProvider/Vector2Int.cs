using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2Int", (int) ConnectorType.ValueProviderVector2Int)]
    public class Vector2Int : ValueProviderBase<UnityEngine.Vector2Int>
    {
        [SerializeField] private UnityEngine.Vector2Int value = default;
        private UnityEngine.Vector2Int Value => value;

        [SerializeField] [ValuePublisher("Value", ValueInjectionType.Vector2Int)] private PublishObjectEvent publisher = default;
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
