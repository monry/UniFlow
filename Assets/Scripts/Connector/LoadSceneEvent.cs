using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniFlow.Connector
{
    [AddComponentMenu("Event Connector/LoadScene", 400)]
    public class LoadSceneEvent : EventPublisher
    {
        [SerializeField] private List<string> sceneNames = default;
        private IEnumerable<string> SceneNames => sceneNames;

        public override IObservable<EventMessage> OnPublishAsObservable()
        {
            return LoadScenes()
                .ToObservable()
                .Select(_ => EventMessage.Create(EventType.LoadScene, this, SceneNames));
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