using JetBrains.Annotations;
using UniFlow.Connector;
using UniFlow.Connector.Controller;

namespace UniFlow.Message
{
    [PublicAPI]
    public class RaycastTargetControllerEventData
    {
        private RaycastTargetControllerEventData(RaycastTargetControlMethod raycastTargetControlMethod)
        {
            RaycastTargetControlMethod = raycastTargetControlMethod;
        }

        public RaycastTargetControlMethod RaycastTargetControlMethod { get; }

        public static RaycastTargetControllerEventData Create(RaycastTargetControlMethod raycastTargetControlMethod)
        {
            return new RaycastTargetControllerEventData(raycastTargetControlMethod);
        }
    }
}