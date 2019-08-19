using System;
using JetBrains.Annotations;
using UniFlow.Message;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector
{
    [AddComponentMenu("UniFlow/AnimatorTrigger", (int) ConnectorType.AnimatorTrigger)]
    public class AnimatorTrigger : ConnectorBase
    {
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<Animator>()")]
        private Animator animator = default;
        [UsedImplicitly] public Animator Animator
        {
            get => animator ? animator : animator = GetComponent<Animator>();
            set => animator = value;
        }

        [SerializeField] private string triggerName = default;
        [UsedImplicitly] public string TriggerName
        {
            get => triggerName;
            set => triggerName = value;
        }

        private int TriggerId => Animator.StringToHash(TriggerName);

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<EventMessage> OnConnectAsObservable()
        {
            return Observable
                .Create<EventMessage>(
                    observer =>
                    {
                        Animator.SetTrigger(TriggerId);
                        observer.OnNext(EventMessage.Create(ConnectorType.AnimatorTrigger, Animator, AnimatorTriggerEventData.Create(TriggerName)));
                        return Disposable;
                    }
                );
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}