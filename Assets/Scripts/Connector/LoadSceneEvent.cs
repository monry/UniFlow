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
    [AddComponentMenu("UniFlow/LoadScene", 400)]
    public class LoadSceneEvent : ConnectorBase
    {
        [SerializeField] private List<string> sceneNames = default;
        private IEnumerable<string> SceneNames
        {
            get => sceneNames;
            [UsedImplicitly]
            set => sceneNames = value.ToList();
        }

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
}