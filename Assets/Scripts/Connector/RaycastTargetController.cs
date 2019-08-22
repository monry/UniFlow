using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/RaycastTargetController", (int) ConnectorType.RaycastTargetController)]
    public class RaycastTargetController : ConnectorBase
    {
        [SerializeField] private List<Graphic> graphics = default;
        [SerializeField] private List<Collider> colliders = default;
        [SerializeField] private List<Collider2D> collider2Ds = default;
        [SerializeField] private RaycastTargetControlMethod raycastTargetControlMethod = default;

        [UsedImplicitly] public IEnumerable<Graphic> Graphics
        {
            get => graphics;
            set => graphics = value.ToList();
        }
        [UsedImplicitly] public IEnumerable<Collider> Colliders
        {
            get => colliders;
            set => colliders = value.ToList();
        }

        [UsedImplicitly] public IEnumerable<Collider2D> Collider2Ds
        {
            get => collider2Ds;
            set => collider2Ds = value.ToList();
        }
        [UsedImplicitly] private RaycastTargetControlMethod RaycastTargetControlMethod
        {
            get => raycastTargetControlMethod;
            set => raycastTargetControlMethod = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        HandleActivation();
                        observer.OnNext(EventMessage.Create(ConnectorType.RaycasterController, this, RaycastTargetControllerEventData.Create(RaycastTargetControlMethod)));
                        return Disposable;
                    }
                );
        }

        private void HandleActivation()
        {
            Graphics.ToList().ForEach(x => x.raycastTarget = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate);
            Colliders.ToList().ForEach(x => x.enabled = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate);
            Collider2Ds.ToList().ForEach(x => x.enabled = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate);
        }
    }

    [PublicAPI]
    public enum RaycastTargetControlMethod
    {
        Activate,
        Deactivate,
    }
}