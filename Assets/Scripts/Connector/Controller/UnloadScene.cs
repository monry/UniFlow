using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/UnloadScene", (int) ConnectorType.UnloadScene)]
    public class UnloadScene : UnloadSceneBase
    {
        [SerializeField] private List<string> sceneNames = default;

        [UsedImplicitly] public override IEnumerable<string> SceneNames
        {
            get => sceneNames;
            set => sceneNames = value.ToList();
        }

        protected override IMessage CreateMessage()
        {
            return Message.Create(this);
        }

        public class Message : MessageBase<UnloadScene>
        {
            public static Message Create(UnloadScene sender)
            {
                return Create<Message>(ConnectorType.UnloadScene, sender);
            }
        }
    }
}