using JetBrains.Annotations;
using UnityEngine;

namespace EventConnector.Message
{
    [PublicAPI]
    public class AnimatorTriggerEvent
    {
        private AnimatorTriggerEvent(int triggerId, string triggerName)
        {
            TriggerId = triggerId;
            TriggerName = triggerName;
        }

        public int TriggerId { get; }
        public string TriggerName { get; }

        public static AnimatorTriggerEvent Create(int triggerId, string triggerName) =>
            new AnimatorTriggerEvent(triggerId, triggerName);

        public static AnimatorTriggerEvent Create(string triggerName) =>
            new AnimatorTriggerEvent(Animator.StringToHash(triggerName), triggerName);
    }
}