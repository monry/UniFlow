using UniFlow.Attribute;
using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/Vector3Int", (int) ConnectorType.ValueProviderVector3Int)]
    public class Vector3Int : Base<UnityEngine.Vector3Int>
    {
        [SerializeField] private UnityEngine.Vector3Int value = default;
        private UnityEngine.Vector3Int Value => value;

        [SerializeField] private PublishVector3IntEvent publisher = default;
        [ValuePublisher("Value")]
        private PublishVector3IntEvent Publisher => publisher ?? (publisher = new PublishVector3IntEvent());

        protected override UnityEngine.Vector3Int Provide()
        {
            Publisher.Invoke(Value);
            return Value;
        }
    }
}
