using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;

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

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        Components.ToList().ForEach(Destroy);
                        GameObjects.ToList().ForEach(Destroy);
                        observer.OnNext(EventMessage.Create(ConnectorType.DestroyInstance, this, DestroyInstanceEventData.Create()));
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