using System;
using EventConnector.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace EventConnector.Connector
{
    public class CameraEvent : EventConnector
    {
        [SerializeField] private CameraEventType cameraEventType;
        private CameraEventType CameraEventType => cameraEventType;

        [SerializeField]
        [Tooltip("If you do not specify it will be used self instance")]
        private Component component;
        private Component Component => component ? component : component = this;

        protected override IObservable<EventMessages> Connect(EventMessages eventMessages)
        {
            return OnEventAsObservable().Select(_ => eventMessages.Append((Component, CameraEventData.Create(CameraEventType))));
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