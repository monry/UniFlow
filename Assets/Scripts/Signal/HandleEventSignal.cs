using System;
using JetBrains.Annotations;

namespace UniFlow.Signal
{
    [PublicAPI]
    public class HandleEventSignal : SignalBase<HandleEventSignal, HandleEventType>
    {
        public HandleEventType HandleEventType { get; private set; }

        protected override HandleEventType ComparableValue
        {
            get => HandleEventType;
            set => HandleEventType = value;
        }
    }

    [PublicAPI]
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
