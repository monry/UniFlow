using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/LoadScene", (int) ConnectorType.LoadScene)]
    public class LoadScene : LoadSceneBase
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

        public class Message : MessageBase<LoadScene>
        {
            public static Message Create(LoadScene sender)
            {
                return Create<Message>(ConnectorType.LoadScene, sender);
            }
        }
    }
}