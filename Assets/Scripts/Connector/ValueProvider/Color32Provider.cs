using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Color32", (int) ConnectorType.ValueProviderColor32)]
    public class Color32Provider : ProviderBase<Color32, PublishColor32Event>
    {
        [SerializeField] private Color32 value = default;
        private Color32 Value => value;

        protected override Color32 Provide()
        {
            return Value;
        }
    }
}
