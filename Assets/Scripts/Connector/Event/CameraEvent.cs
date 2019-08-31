using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector.Event
{
    [AddComponentMenu("UniFlow/Event/CameraEvent", (int) ConnectorType.CameraEvent)]
    public class CameraEvent : ConnectorBase
    {
        [SerializeField] private CameraEventType cameraEventType = (CameraEventType) (-1);
        [SerializeField] private Component component = default;

        [UsedImplicitly] public CameraEventType CameraEventType
        {
            get => cameraEventType;
            set => cameraEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
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