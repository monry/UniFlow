using JetBrains.Annotations;
using UniFlow.Connector;

namespace UniFlow.Message
{
    [PublicAPI]
    public class RaycasterControllerEventData
    {
        private RaycasterControllerEventData(RaycasterControlMethod raycasterControlMethod)
        {
            RaycasterControlMethod = raycasterControlMethod;
        }

        public RaycasterControlMethod RaycasterControlMethod { get; }

        public static RaycasterControllerEventData Create(RaycasterControlMethod raycasterControlMethod)
        {
            return new RaycasterControllerEventData(raycasterControlMethod);
        }
    }
}