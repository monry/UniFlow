using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return Observable
                .Create<IMessage>(
                    observer =>
                    {
                        var count = 0;
                        var monoBehaviours = MonoBehaviours.ToList();
                        count += monoBehaviours.Count;
                        monoBehaviours.ForEach(x => x.enabled = Activated);
                        var gameObjects = GameObjects.ToList();
                        count += gameObjects.Count();
                        gameObjects.ForEach(x => x.SetActive(Activated));
                        observer.OnNext(Message.Create(this, count));
                        return Disposable;
                    }
                );
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }

        public class Message : MessageBase<ActivationController, int>
        {
            public static Message Create(ActivationController sender, int count)
            {
                return Create<Message>(ConnectorType.ActivationController, sender, count);
            }
        }
    }
}