using System;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/AnimatorTrigger", (int) ConnectorType.AnimatorTrigger)]
    public class AnimatorTrigger : ConnectorBase
    {
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<Animator>()")]
        private Animator animator = default;
        [SerializeField] private string triggerName = default;

        [UsedImplicitly] public Animator Animator
        {
            get => animator ? animator : animator = GetComponent<Animator>();
            set => animator = value;
        }
        [UsedImplicitly] public string TriggerName
        {
            get => triggerName;
            set => triggerName = value;
        }

        private int TriggerId => Animator.StringToHash(TriggerName);

        private IDisposable Disposable { get; } = new CompositeDisposable();

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Animator.SetTrigger(TriggerId);
            return Observable.ReturnUnit();
        }

        private void OnDestroy()
        {
            Disposable.Dispose();
        }
    }
}
