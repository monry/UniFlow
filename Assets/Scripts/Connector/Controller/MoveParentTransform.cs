using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/MoveParentTransform", (int) ConnectorType.MoveParentTransform)]
    public class MoveParentTransform : ConnectorBase
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private Transform parentTransform = default;
        [SerializeField] private bool worldPositionStays = true;

        [UsedImplicitly]
        [ValueReceiver]
        public Transform TargetTransform
        {
            get => targetTransform != default
                ? targetTransform
                : targetTransform = transform;
            set => targetTransform = value;
        }
        [UsedImplicitly]
        [ValueReceiver]
        public Transform ParentTransform
        {
            get => parentTransform;
            set => parentTransform = value;
        }
        [UsedImplicitly] public bool WorldPositionStays
        {
            get => worldPositionStays;
            set => worldPositionStays = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            TargetTransform.SetParent(ParentTransform, WorldPositionStays);
            return Observable.ReturnUnit();
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}
