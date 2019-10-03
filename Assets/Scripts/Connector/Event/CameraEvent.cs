using System;
using JetBrains.Annotations;
using UniFlow.Attribute;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/CameraEvent", (int) ConnectorType.CameraEvent)]
    public class CameraEvent : ConnectorBase
    {
        [SerializeField] private Component component = default;
        [SerializeField] private CameraEventType cameraEventType = CameraEventType.BecomeVisible;

        [ValueReceiver] public Component Component
        {
            get => component ? component : component = this;
            set => component = value;
        }
        [UsedImplicitly] public CameraEventType CameraEventType
        {
            get => cameraEventType;
            set => cameraEventType = value;
        }

        public override IObservable<Unit> OnConnectAsObservable()
        {
            return OnEventAsObservable();
        }

        private IObservable<Unit> OnEventAsObservable()
        {
            switch (CameraEventType)
            {
                case CameraEventType.BecomeVisible:
                    return Component.OnBecameVisibleAsObservable();
                case CameraEventType.BecomeInvisible:
                    return Component.OnBecameInvisibleAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum CameraEventType
    {
        BecomeVisible,
        BecomeInvisible,
    }
}
