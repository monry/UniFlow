using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/UnloadScene", (int) ConnectorType.UnloadScene)]
    public class UnloadSceneEvent : ConnectorBase
    {
        [SerializeField] private List<string> sceneNames = default;

        [UsedImplicitly] public IEnumerable<string> SceneNames
        {
            get => sceneNames;
            set => sceneNames = value.ToList();
        }

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
}