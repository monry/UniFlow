using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/UnloadScene", 401)]
    public class UnloadSceneEvent : ConnectorBase
    {
        [SerializeField] private List<string> sceneNames = default;
        private IEnumerable<string> SceneNames => sceneNames;

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