using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/ParticleEvent", (int) ConnectorType.ParticleEvent)]
    public class ParticleEvent : ConnectorBase
    {
        [SerializeField] private ParticleEventType particleEventType = (ParticleEventType) (-1);
        [SerializeField] private Component component = default;

        [UsedImplicitly] public ParticleEventType ParticleEventType
        {
            get => particleEventType;
            set => particleEventType = value;
        }
        [UsedImplicitly] public Component Component
        {
            get => component ? component : component = this;
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