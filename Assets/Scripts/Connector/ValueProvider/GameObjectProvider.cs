using UnityEngine;

namespace UniFlow.Connector.ValueProvider
{
    [AddComponentMenu("UniFlow/ValueProvider/GameObject", (int) ConnectorType.ValueProviderGameObject)]
    public class GameObjectProvider : ProviderBase<GameObject, PublishGameObjectEvent>
    {
    }
}
