using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/String", (int) ConnectorType.ValueProviderString)]
    public class StringProvider : ProviderBase<string, PublishStringEvent>
    {
        [SerializeField] private string value = default;
        private string Value => value;

        protected override string Provide()
        {
            return Value;
        }
    }
}
