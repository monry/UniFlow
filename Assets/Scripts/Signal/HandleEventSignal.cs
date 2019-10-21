using System;

namespace UniFlow.Signal
{
    public class HandleEventSignal : SignalBase<HandleEventSignal, HandleEventType>
    {
        public HandleEventType HandleEventType { get; private set; }

        protected override HandleEventType CreateComparableValue()
        {
            return HandleEventType;
        }

        public static HandleEventSignal Create(HandleEventType handleEventType)
        {
            return new HandleEventSignal {HandleEventType = handleEventType};
        }
    }

    public enum HandleEventType
    {
        Activate,
        Deactivate,
    }

    [Serializable]
    public class HandleEventTypeCollector : ValueCollectorBase<HandleEventType>
    {
    }
}
