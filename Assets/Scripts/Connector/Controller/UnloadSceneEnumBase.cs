using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace UniFlow.Connector.Controller
{
    public abstract class UnloadScene<TSceneName> : UnloadSceneBase where TSceneName : Enum
    {
        [SerializeField] private List<TSceneName> sceneNames = default;

        [InjectOptional(Id = InjectId.SceneNamePrefix)] private string SceneNamePrefix { get; }

        [UsedImplicitly] public override IEnumerable<string> SceneNames
        {
            get => sceneNames.Select(x => $"{SceneNamePrefix}{x.ToString()}");
            set => sceneNames = value
                .Select(
                    x => (TSceneName) Enum
                        .Parse(
                            typeof(TSceneName),
                            Regex.Replace(x, $"^{SceneNamePrefix}", string.Empty)
                        )
                )
                .ToList();
        }

        protected override IMessage CreateMessage()
        {
            return Message.Create(this);
        }

        public class Message : MessageBase<UnloadScene<TSceneName>>
        {
            public static Message Create(UnloadScene<TSceneName> sender)
            {
                return Create<Message>(ConnectorType.UnloadScene_Enum, sender);
            }
        }
    }
}