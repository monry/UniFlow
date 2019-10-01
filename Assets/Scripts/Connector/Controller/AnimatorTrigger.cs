using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/AnimatorTrigger", (int) ConnectorType.AnimatorTrigger)]
    public class AnimatorTrigger : ConnectorBase
    {
        [SerializeField] private string triggerName = default;
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<Animator>()")]
        private Animator animator = default;

        [ValueReceiver] public string TriggerName
        {
            get => triggerName;
            set => triggerName = value;
        }
        [ValueReceiver] public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public Animator Animator
        {
            get => animator ? animator : animator = BaseGameObject.GetComponent<Animator>();
            set => animator = value;
        }

        private int TriggerId => Animator.StringToHash(TriggerName);

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Animator.SetTrigger(TriggerId);
            return Observable.ReturnUnit();
        }
    }
}
