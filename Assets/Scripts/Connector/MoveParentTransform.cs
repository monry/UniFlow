using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/MoveParentTransform", (int) ConnectorType.MoveParentTransform)]
    public class MoveParentTransform : ConnectorBase
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField] private Transform _transform = default;
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private bool worldPositionStays = true;

        [UsedImplicitly] public Transform Transform
        {
            get => _transform != default
                ? _transform
                : _transform = transform;
            set => _transform = value;
        }
        [UsedImplicitly] public Transform TargetTransform
        {
            get => targetTransform;
            set => targetTransform = value;
        }
        [UsedImplicitly] public bool WorldPositionStays
        {
            get => worldPositionStays;
            set => worldPositionStays = value;
        }

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        Transform.SetParent(TargetTransform, WorldPositionStays);
                        observer.OnNext(EventMessage.Create(ConnectorType.MoveParentTransform, this, MoveParentTransformEventData.Create(TargetTransform)));
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