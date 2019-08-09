using System;
using UniFlow.Message;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/AnimatorTrigger", 301)]
    public class AnimatorTrigger : EventPublisher
    {
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<Animator>()")]
        private Animator animator = default;
        [SerializeField] private string triggerName = default;

        private Animator Animator => animator ? animator : animator = GetComponent<Animator>();
        private string TriggerName => triggerName;
        private int TriggerId => Animator.StringToHash(TriggerName);

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<EventMessage> OnPublishAsObservable() =>
            Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        Animator.SetTrigger(TriggerId);
                        observer.OnNext(EventMessage.Create(EventType.AnimatorTrigger, Animator, AnimatorTriggerEventData.Create(TriggerName)));
                        return Disposable;
                    }
                );

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}