using System;

namespace UniFlow
{
    public interface ISignal
    {
    }

    public interface IEquatableSignal<TSignal> : ISignal, IEquatable<TSignal>
    {
    }
}
