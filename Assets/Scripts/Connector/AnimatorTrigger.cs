using EventConnector.Message;
using UniRx;
using UnityEngine;

namespace EventConnector.Connector
{
    [AddComponentMenu("Event Connector/AnimatorTrigger")]
    public class AnimatorTrigger : EventConnector
    {
        [SerializeField]
        [Tooltip("If you do not specify it will be obtained by GameObject.GetComponent<Animator>()")]
        private Animator animator = default;
        [SerializeField] private string triggerName = default;

        private Animator Animator => animator ? animator : animator = GetComponent<Animator>();
        private string TriggerName => triggerName;
        private int TriggerId => Animator.StringToHash(TriggerName);

        protected override void Connect(EventMessages eventMessages)
        {
            Animator.SetTrigger(TriggerId);
            Observable
                .EveryEndOfFrame()
                .Take(1)
                .SubscribeWithState(
                    eventMessages,
                    (_, em) => OnConnect(em.Append(EventMessage.Create(EventType.AnimatorTrigger, Animator, AnimatorTriggerEventData.Create(TriggerName))))
                );
        }
    }
}