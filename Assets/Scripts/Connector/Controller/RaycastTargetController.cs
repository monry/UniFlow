using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/RaycastTargetController", (int) ConnectorType.RaycastTargetController)]
    public class RaycastTargetController : ConnectorBase
    {
        [SerializeField] private RaycastTargetControlMethod raycastTargetControlMethod = RaycastTargetControlMethod.Activate;
        [SerializeField] private List<Graphic> graphics = default;
        [SerializeField] private List<Collider> colliders = default;
        [SerializeField] private List<Collider2D> collider2Ds = default;
        [SerializeField] private List<CanvasGroup> canvasGroups = default;

        [UsedImplicitly] private RaycastTargetControlMethod RaycastTargetControlMethod
        {
            get => raycastTargetControlMethod;
            set => raycastTargetControlMethod = value;
        }
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
        [UsedImplicitly] public IEnumerable<CanvasGroup> CanvasGroups
        {
            get => canvasGroups;
            set => canvasGroups = value.ToList();
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            HandleActivation();
            return Observable.ReturnUnit();
        }

        private void HandleActivation()
        {
            var handleTargets = new Action[0]
                .Concat(
                    Graphics
                        .Where(x => x.raycastTarget != (RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                        .Select(x => new Action(() => x.raycastTarget = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                )
                .Concat(
                    Colliders
                        .Where(x => x.enabled != (RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                        .Select(x => new Action(() => x.enabled = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                )
                .Concat(
                    Collider2Ds
                        .Where(x => x.enabled != (RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                        .Select(x => new Action(() => x.enabled = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                )
                .Concat(
                    CanvasGroups
                        .Where(x => x.blocksRaycasts != (RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                        .Select(x => new Action(() => x.blocksRaycasts = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                )
                .Concat(
                    GetComponents<Graphic>()
                        .Where(x => x.raycastTarget != (RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                        .Select(x => new Action(() => x.raycastTarget = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                )
                .Concat(
                    GetComponents<Collider>()
                        .Where(x => x.enabled != (RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                        .Select(x => new Action(() => x.enabled = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                )
                .Concat(
                    GetComponents<Collider2D>()
                        .Where(x => x.enabled != (RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                        .Select(x => new Action(() => x.enabled = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                )
                .Concat(
                    GetComponents<CanvasGroup>()
                        .Where(x => x.blocksRaycasts != (RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                        .Select(x => new Action(() => x.blocksRaycasts = RaycastTargetControlMethod == RaycastTargetControlMethod.Activate))
                )
                .ToList();
            handleTargets.ForEach(x => x.Invoke());
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }

    [PublicAPI]
    public enum RaycastTargetControlMethod
    {
        Activate,
        Deactivate,
    }
}
