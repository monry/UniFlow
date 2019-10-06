using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color", (int) ConnectorType.ValueProviderColor)]
    public class ColorProvider : ValueProviderBase<Color, PublishColorEvent>
    {
        protected override Color Provide()
        {
            return Value;
        }
    }
}
