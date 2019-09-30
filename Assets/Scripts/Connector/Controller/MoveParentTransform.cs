using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/MoveParentTransform", (int) ConnectorType.MoveParentTransform)]
    public class MoveParentTransform : ConnectorBase
    {
        // ReSharper disable once InconsistentNaming
        [SerializeField] private Transform targetTransform = default;
        [SerializeField] private Transform parentTransform = default;
        [SerializeField] private bool worldPositionStays = true;

        [UsedImplicitly] public Transform TargetTransform
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

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<IMessage> OnConnectAsObservable(IMessage latestMessage)
        {
            TargetTransform.SetParent(ParentTransform, WorldPositionStays);
            return Observable.Return(Message.Create(this, TargetTransform));
        }

        [ValueReceiver("Target Transform", ValueInjectionType.Transform)]
        public void ReceiveTargetTransform(Object target)
        {
            TargetTransform = target as Transform;
        }

        [ValueReceiver("Parent Transform", ValueInjectionType.Transform)]
        public void ReceiveParentTransform(Object parent)
        {
            ParentTransform = parent as Transform;
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
