using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UniRx.Async;
using UnityEngine.SceneManagement;

namespace UniFlow.Connector.Controller
{
    public abstract class LoadSceneBase : ConnectorBase
    {
        [UsedImplicitly]
        public abstract IEnumerable<string> SceneNames { get; set; }

        public override IObservable<Message> OnConnectAsObservable()
        {
            return LoadScenes()
                .ToObservable()
                .Select(this.CreateMessage);
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
