using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color", (int) ConnectorType.ValueProviderColor)]
    public class ColorProvider : ProviderBase<Color, PublishColorEvent>
    {
        [SerializeField] private Color value = default;
        private Color Value => value;

        protected override Color Provide()
        {
            return Value;
        }
    }
}
