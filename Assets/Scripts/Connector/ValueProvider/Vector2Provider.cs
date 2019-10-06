using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector2", (int) ConnectorType.ValueProviderVector2)]
    public class Vector2Provider : ValueProviderBase<Vector2, PublishVector2Event>
    {
        protected override Vector2 Provide()
        {
            return Value;
        }
    }
}
