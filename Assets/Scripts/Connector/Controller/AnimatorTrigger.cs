using System;
using UniFlow.Attribute;
using UniRx;
using UnityEngine;

namespace UniFlow.Connector.Controller
{
    [AddComponentMenu("UniFlow/Controller/AnimatorTrigger", (int) ConnectorType.AnimatorTrigger)]
    public class AnimatorTrigger : ConnectorBase, IBaseGameObjectSpecifyable
    {
        [SerializeField] private GameObject baseGameObject = default;
        [SerializeField] private string transformPath = default;
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<Animator>()")]
        private Animator animator = default;
        [SerializeField] private string triggerName = default;

        [ValueReceiver] public GameObject BaseGameObject
        {
            get => baseGameObject == default ? baseGameObject = gameObject : baseGameObject;
            set => baseGameObject = value;
        }
        [ValueReceiver] public string TransformPath
        {
            get => transformPath;
            set => transformPath = value;
        }
        [ValueReceiver] public Animator Animator
        {
            get => animator ? animator : animator = this.GetOrAddComponent<Animator>();
            set => animator = value;
        }
        [ValueReceiver] public string TriggerName
        {
            get => triggerName;
            set => triggerName = value;
        }

        private int TriggerId => Animator.StringToHash(TriggerName);

        public override IObservable<Unit> OnConnectAsObservable()
        {
            Animator.SetTrigger(TriggerId);
            return Observable.ReturnUnit();
        }
    }
}
