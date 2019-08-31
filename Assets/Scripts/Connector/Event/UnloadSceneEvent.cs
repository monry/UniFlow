using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UniFlow.Connector.Event
{
    public abstract class UnloadSceneEventBase : ConnectorBase
    {
        [UsedImplicitly]
        public abstract IEnumerable<string> SceneNames { get; set; }

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return UnloadScenes()
                .ToObservable()
                .Select(_ => EventMessage.Create(ConnectorType.UnloadScene, this, SceneNames));
        }

        private async UniTask UnloadScenes()
        {
            foreach (var sceneName in SceneNames)
            {
                await SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }

    [AddComponentMenu("UniFlow/Event/UnloadScene", (int) ConnectorType.UnloadScene)]
    public class UnloadSceneEvent : UnloadSceneEventBase
    {
        [SerializeField] private List<string> sceneNames = default;

        [UsedImplicitly] public override IEnumerable<string> SceneNames
        {
            get => sceneNames;
            set => sceneNames = value.ToList();
        }
    }

    public abstract class UnloadSceneEvent<TSceneName> : UnloadSceneEventBase where TSceneName : Enum
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
    }
}