using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/RaycasterController", (int) ConnectorType.RaycasterController)]
    public class RaycasterController : ConnectorBase
    {
        [SerializeField] private RaycasterControlMethod raycasterControlMethod = (RaycasterControlMethod) (-1);
        [SerializeField] private List<BaseRaycaster> raycasters = default;

        [UsedImplicitly] private RaycasterControlMethod RaycasterControlMethod
        {
            get => raycasterControlMethod;
            set => raycasterControlMethod = value;
        }
        [UsedImplicitly] private IEnumerable<BaseRaycaster> Raycasters
        {
            get => raycasters;
            set => raycasters = value.ToList();
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        HandleActivation();
                        observer.OnNext(EventMessage.Create(ConnectorType.RaycasterController, this, RaycasterControllerEventData.Create(RaycasterControlMethod)));
                        return Disposable;
                    }
                );
        }

        private void HandleActivation()
        {
            Raycasters.ToList().ForEach(x => x.enabled = RaycasterControlMethod == RaycasterControlMethod.Activate);
        }
    }

    [PublicAPI]
    public enum RaycasterControlMethod
    {
        Activate,
        Deactivate,
    }
}