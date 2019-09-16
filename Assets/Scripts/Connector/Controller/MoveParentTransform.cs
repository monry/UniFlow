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

        [SerializeField][SuppliableType(typeof(GameObject))] private ConnectorBase targetGameObjectProvider = default;
        [SerializeField][SuppliableType(typeof(GameObject))] private ConnectorBase parentGameObjectProvider = default;

        [UsedImplicitly] public Transform Transform
        {
            get => targetTransform != default
                ? targetTransform
                : targetTransform = transform;
            set => targetTransform = value;
        }
        [UsedImplicitly] public Transform ParentTransform
        {
            get => parentTransform;
            set => parentTransform = value;
        }
        [UsedImplicitly] public bool WorldPositionStays
        {
            get => worldPositionStays;
            set => worldPositionStays = value;
        }
        [UsedImplicitly] public IValueProvider<GameObject> TargetGameObjectProvider => targetGameObjectProvider as IValueProvider<GameObject>;
        [UsedImplicitly] public IValueProvider<GameObject> ParentGameObjectProvider => parentGameObjectProvider as IValueProvider<GameObject>;

        private IDisposable Disposable { get; } = new CompositeDisposable();

        protected override void CollectSuppliedValues()
        {
            TargetGameObjectProvider?.OnProvideAsObservable().Subscribe(x => Transform = x.transform);
            ParentGameObjectProvider?.OnProvideAsObservable().Subscribe(x => ParentTransform = x.transform);
        }

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            return Observable
                .Create<IMessage>(
                    observer =>
                    {
                        Transform.SetParent(ParentTransform, WorldPositionStays);
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