using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UniRx.Async;
using UnityEngine.SceneManagement;
using Zenject;

namespace UniFlow.Connector.Controller
{
    public abstract class UnloadSceneBase : ConnectorBase
    {
        [UsedImplicitly]
        public abstract IEnumerable<string> SceneNames { get; set; }

        [InjectOptional] private AsyncUnloadSceneDelegate AsyncUnloadSceneDelegate { get; } = DefaultAsyncUnloadScene;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return AsyncUnloadSceneDelegate(SceneNames)
                .ToObservable()
                .Select(this.CreateMessage);
        }

        private static async UniTask DefaultAsyncUnloadScene(IEnumerable<string> sceneNames)
        {
            foreach (var sceneName in sceneNames)
            {
                await SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }

    public delegate UniTask AsyncUnloadSceneDelegate(IEnumerable<string> sceneNames);
}
