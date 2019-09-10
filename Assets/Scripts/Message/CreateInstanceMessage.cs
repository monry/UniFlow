using UnityEngine;

namespace UniFlow.Message
{
    public class CreateInstanceMessage : EventMessage2<GameObject>, IValueHolder<GameObject>
    {
        private CreateInstanceMessage(ConnectorType connectorType, GameObject data) : base(connectorType, data)
        {
        }

        GameObject IValueHolder<GameObject>.Value => Data;

        public static CreateInstanceMessage Create(object sender)
        {
            return new CreateInstanceMessage(
                ConnectorType.Custom,
                sender is IValuePublisher<GameObject> gameObjectPublisher ? gameObjectPublisher.Publish() : null
            );
        }

    }
}