using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/UnloadSceneEvent", (int) ConnectorType.UnloadSceneEvent)]
    public class UnloadSceneEvent : ConnectorBase
    {
        [SerializeField] private string sceneName = default;

        [UsedImplicitly] public string SceneName
        {
            get => sceneName;
            set => sceneName = value;
        }

        private ISubject<Unit> Subject { get; } = new Subject<Unit>();

        public override IObservable<Unit> OnConnectAsObservable() => Subject;

        protected override void Start()
        {
            base.Start();
            SceneManager.sceneUnloaded += DidUnloadScene;
        }

        private void OnDestroy()
        {
            SceneManager.sceneUnloaded -= DidUnloadScene;
        }

        private void DidUnloadScene(Scene unloadedScene)
        {
            if (unloadedScene.name == SceneName)
            {
                Subject.OnNext(Unit.Default);
            }
        }
    }
}
