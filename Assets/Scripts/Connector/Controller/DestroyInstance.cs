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

        public override IObservable<Unit> OnConnectAsObservable()
        {
            var targets = Components.Concat<Object>(GameObjects).ToList();
            targets.ToList().ForEach(Destroy);
            return Observable.ReturnUnit();
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}
