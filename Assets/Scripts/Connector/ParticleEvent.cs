using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/ParticleEvent", 105)]
    public class ParticleEvent : ConnectorBase
    {
        [SerializeField] private ParticleEventType particleEventType = default;
        private ParticleEventType ParticleEventType => particleEventType;

        private Component component = default;
        private Component Component
        {
            get => component ? component : component = this;
            [UsedImplicitly]
            set => component = value;
        }

        public override IObservable<EventMessage> OnConnectAsObservable() =>
            OnEventAsObservable()
                .Select(x => EventMessage.Create(ConnectorType.ParticleEvent, Component, ParticleEventData.Create(ParticleEventType, x)));

        private IObservable<GameObject> OnEventAsObservable()
        {
            switch (ParticleEventType)
            {
                case ParticleEventType.ParticleCollision:
                    return Component.OnParticleCollisionAsObservable();
                case ParticleEventType.ParticleTrigger:
                    return Component.OnParticleTriggerAsObservable().Select(_ => Component.gameObject);
// TODO: Uncomment when the ObservableParticleSystemStoppedTrigger is implemented
//                case ParticleEventType.ParticleSystemStopped:
//                    return Component.OnParticleSystemStoppedAsObservable();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public enum ParticleEventType
    {
        ParticleCollision,
        ParticleTrigger,
//        ParticleSystemStopped,
    }
}