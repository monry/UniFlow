using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/LoadSceneEvent", (int) ConnectorType.LoadSceneEvent)]
    public class LoadSceneEvent : ConnectorBase
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
            SceneManager.sceneLoaded += DidLoadScene;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= DidLoadScene;
        }

        private void DidLoadScene(Scene loadedScene, LoadSceneMode mode)
        {
            if (loadedScene.name == SceneName)
            {
                Subject.OnNext(Unit.Default);
            }
        }
    }
}
