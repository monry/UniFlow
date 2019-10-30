using System;

namespace UniFlow
{
    public interface ISignal
    {
    }

    public interface ISignal<TSignal> : ISignal, IEquatable<TSignal>
    {
    }
}
