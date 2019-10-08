using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/GameObject", (int) ConnectorType.ValueProviderGameObject)]
    public class GameObjectProvider : ValueProviderBase<GameObject, PublishGameObjectEvent>
    {
        protected override GameObject Provide()
        {
            return Value;
        }
    }
}
