using JetBrains.Annotations;
using UnityEngine;

namespace UniFlow.Message
{
    [PublicAPI]
    public class MoveParentTransformEventData
    {
        private MoveParentTransformEventData(Transform targetTransform)
        {
            TargetTransform = targetTransform;
        }

        public Transform TargetTransform { get; }

        public static MoveParentTransformEventData Create(Transform targetTransform)
        {
            return new MoveParentTransformEventData(targetTransform);
        }
    }
}