using JetBrains.Annotations;
using UnityEngine;

namespace EventConnector.Message
{
    [PublicAPI]
    public class AnimatorTriggerEventData
    {
        private AnimatorTriggerEventData(int triggerId, string triggerName)
        {
            TriggerId = triggerId;
            TriggerName = triggerName;
        }

        public int TriggerId { get; }
        public string TriggerName { get; }

        public static AnimatorTriggerEventData Create(int triggerId, string triggerName) =>
            new AnimatorTriggerEventData(triggerId, triggerName);

        public static AnimatorTriggerEventData Create(string triggerName) =>
            new AnimatorTriggerEventData(Animator.StringToHash(triggerName), triggerName);
    }
}