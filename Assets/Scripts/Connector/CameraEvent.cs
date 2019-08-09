using System;
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
        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component = default;

        private CameraEventType CameraEventType => cameraEventType;
        private Component Component => component ? component : component = this;

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            OnEventAsObservable()
                .Select(_ => EventMessage.Create(ConnectorType.CameraEvent, Component, CameraEventData.Create(CameraEventType)));

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