using JetBrains.Annotations;
using UniFlow.Connector;
using UniFlow.Connector.Controller;

namespace UniFlow.Message
{
    [PublicAPI]
    public struct SimpleAnimationControllerEventData
    {
        private SimpleAnimationControllerEventData(SimpleAnimationControlMethod simpleAnimationControlMethod)
        {
            SimpleAnimationControlMethod = simpleAnimationControlMethod;
        }

        public SimpleAnimationControlMethod SimpleAnimationControlMethod { get; }

        public static SimpleAnimationControllerEventData Create(SimpleAnimationControlMethod audioControlMethod) =>
            new SimpleAnimationControllerEventData(audioControlMethod);
    }
}