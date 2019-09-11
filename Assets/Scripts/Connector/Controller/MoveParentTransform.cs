using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/MoveParentTransform", (int) ConnectorType.MoveParentTransform)]
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

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return Observable
                .Create<IMessage>(
                    observer =>
                    {
                        Transform.SetParent(TargetTransform, WorldPositionStays);
                        observer.OnNext(Message.Create(this, Transform));
                        return Disposable;
                    }
                );
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }

        public class Message : MessageBase<MoveParentTransform, Transform>
        {
            public static Message Create(MoveParentTransform sender, Transform movedTransform)
            {
                return Create<Message>(ConnectorType.MoveParentTransform, sender, movedTransform);
            }
        }
    }
}