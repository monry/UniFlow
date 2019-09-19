using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UniRx;
using UniRx.Async;
using UnityEngine.SceneManagement;

namespace UniFlow.Connector.Controller
{
    public abstract class UnloadSceneBase : ConnectorBase
    {
        [UsedImplicitly]
        public abstract IEnumerable<string> SceneNames { get; set; }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return UnloadScenes()
                .ToObservable()
                .Select(_ => CreateMessage());
        }

        protected abstract IMessage CreateMessage();

        private async UniTask UnloadScenes()
        {
            foreach (var sceneName in SceneNames)
            {
                await SceneManager.UnloadSceneAsync(sceneName);
            }
        }
    }
}