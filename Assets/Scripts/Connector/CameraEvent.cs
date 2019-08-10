using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/CameraEvent", 104)]
    public class CameraEvent : ConnectorBase
    {
        [SerializeField] private CameraEventType cameraEventType = default;
        private CameraEventType CameraEventType => cameraEventType;

        private Component component = default;
        private Component Component
        {
            get => component ? component : component = this;
            [UsedImplicitly]
            set => component = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return OnEventAsObservable()
                .Select(_ => EventMessage.Create(ConnectorType.CameraEvent, Component, CameraEventData.Create(CameraEventType)));
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