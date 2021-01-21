using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UniRx;
using UnityEngine.SceneManagement;
using Zenject;

namespace UniFlow.Connector.Controller
{
    public abstract class LoadSceneBase : ConnectorBase
    {
        [UsedImplicitly]
        public abstract IEnumerable<string> SceneNames { get; set; }

        [InjectOptional] private AsyncLoadSceneDelegate AsyncLoadSceneDelegate { get; } = DefaultAsyncLoadScene;

        public override IObservable<Message> OnConnectAsObservable()
        {
            return AsyncLoadSceneDelegate(SceneNames)
                .ToObservable()
                .Select(this.CreateMessage);
        }

        private static async UniTask DefaultAsyncLoadScene(IEnumerable<string> sceneNames)
        {
            foreach (var sceneName in sceneNames)
            {
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
        }
    }

    public delegate UniTask AsyncLoadSceneDelegate(IEnumerable<string> sceneNames);
}
