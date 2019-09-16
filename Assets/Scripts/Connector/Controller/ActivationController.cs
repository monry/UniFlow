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
        [SerializeField] private List<GameObject> targetGameObjects = default;
        [SerializeField] private List<MonoBehaviour> targetMonoBehaviours = default;
        [SerializeField] private bool controlSelfInstance = default;
        [SerializeField] private bool activated = default;

        [UsedImplicitly] public IEnumerable<GameObject> GameObjects
        {
            get => targetGameObjects;
            set => targetGameObjects = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<MonoBehaviour> MonoBehaviours
        {
            get => targetMonoBehaviours;
            set => targetMonoBehaviours = value.ToList();
        }
        [UsedImplicitly] public bool ControlSelfInstance
        {
            get => controlSelfInstance;
            set => controlSelfInstance = value;
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
                        var gameObjects = GameObjects.Where(x => x.activeSelf != Activated).ToList();
                        count += gameObjects.Count;
                        gameObjects.ForEach(x => x.SetActive(Activated));
                        var monoBehaviours = MonoBehaviours.Where(x => x.enabled != Activated).ToList();
                        count += monoBehaviours.Count;
                        monoBehaviours.ForEach(x => x.enabled = Activated);
                        if (ControlSelfInstance)
                        {
                            if (gameObject.activeSelf != Activated)
                            {
                                count++;
                                gameObject.SetActive(Activated);
                            }
                            var components = GetComponents<MonoBehaviour>().Where(x => x.enabled != Activated).ToList();
                            count += components.Count;
                            components.ForEach(x => x.enabled = Activated);
                        }
                        observer.OnNext(Message.Create(this, count, gameObjects, monoBehaviours));
                        return Disposable;
                    }
                );
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }

        public class Message : MessageBase<ActivationController, (int count, IEnumerable<GameObject> gameObjects, IEnumerable<MonoBehaviour> monoBehaviours)>
        {
            public static Message Create(ActivationController sender, int count, IEnumerable<GameObject> gameObjects, IEnumerable<MonoBehaviour> monoBehaviours)
            {
                return Create<Message>(ConnectorType.ActivationController, sender, (count, gameObjects, monoBehaviours));
            }
        }
    }
}