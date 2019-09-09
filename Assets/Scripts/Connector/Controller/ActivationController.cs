using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/ActivationController", (int) ConnectorType.LoadScene)]
    public class ActivationController : ConnectorBase
    {
        [SerializeField] private List<MonoBehaviour> targetMonoBehaviours = default;
        [SerializeField] private List<GameObject> targetGameObjects = default;
        [SerializeField] private bool activated = default;

        [UsedImplicitly] public IEnumerable<MonoBehaviour> MonoBehaviours
        {
            get => targetMonoBehaviours;
            set => targetMonoBehaviours = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<GameObject> GameObjects
        {
            get => targetGameObjects;
            set => targetGameObjects = value.ToList();
        }
        [UsedImplicitly] public bool Activated
        {
            get => activated;
            set => activated = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        MonoBehaviours.ToList().ForEach(x => x.enabled = Activated);
                        GameObjects.ToList().ForEach(x => x.SetActive(Activated));
                        observer.OnNext(EventMessage.Create(ConnectorType.ActivationController, this, ActivationControllerEventData.Create(Activated)));
                        return Disposable;
                    }
                );
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}