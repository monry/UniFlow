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

namespace UniFlow.Connector.Controller
{
    public abstract class LoadSceneEventBase : ConnectorBase
    {
        [UsedImplicitly]
        public abstract IEnumerable<string> SceneNames { get; set; }

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return LoadScenes()
                .ToObservable()
                .Select(_ => EventMessage.Create(ConnectorType.LoadScene, this, SceneNames));
        }

        private async UniTask LoadScenes()
        {
            foreach (var sceneName in SceneNames)
            {
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
        }
    }

    [AddComponentMenu("UniFlow/Controller//LoadScene", (int) ConnectorType.LoadScene)]
    public class LoadSceneEvent : LoadSceneEventBase
    {
        [SerializeField] private List<string> sceneNames = default;

        [UsedImplicitly] public override IEnumerable<string> SceneNames
        {
            get => sceneNames;
            set => sceneNames = value.ToList();
        }
    }

    public abstract class LoadSceneEvent<TSceneName> : LoadSceneEventBase where TSceneName : Enum
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