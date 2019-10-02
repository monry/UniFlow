using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Int", (int) ConnectorType.ValueProviderInt)]
    public class IntProvider : ProviderBase<int, PublishIntEvent>
    {
        [SerializeField] private int value = default;
        private int Value => value;

        protected override int Provide()
        {
            return Value;
        }
    }
}
