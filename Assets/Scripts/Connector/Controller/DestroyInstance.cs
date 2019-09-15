using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/DestroyInstance", (int) ConnectorType.LoadScene)]
    public class DestroyInstance : ConnectorBase
    {
        [SerializeField] private List<Component> targetComponents = default;
        [SerializeField] private List<GameObject> targetGameObjects = default;

        [UsedImplicitly] public IEnumerable<Component> Components
        {
            get => targetComponents;
            set => targetComponents = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<GameObject> GameObjects
        {
            get => targetGameObjects;
            set => targetGameObjects = value.ToList();
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return Observable
                .Create<IMessage>(
                    observer =>
                    {
                        var targets = Components.Concat<Object>(GameObjects).ToList();
                        var count = targets.Count;
                        targets.ToList().ForEach(Destroy);
                        observer.OnNext(Message.Create(this, count));
                        return Disposable;
                    }
                );
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }

        public class Message : MessageBase<DestroyInstance, int>
        {
            public static Message Create(DestroyInstance sender, int count)
            {
                return Create<Message>(ConnectorType.DestroyInstance, sender, count);
            }
        }
    }
}