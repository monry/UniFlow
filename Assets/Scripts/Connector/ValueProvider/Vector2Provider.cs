using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2", (int) ConnectorType.ValueProviderVector2)]
    public class Vector2Provider : ProviderBase<Vector2, PublishVector2Event>
    {
        [SerializeField] private Vector2 value = default;
        private Vector2 Value => value;

        protected override Vector2 Provide()
        {
            return Value;
        }
    }
}
