using System;

namespace UniFlow
{
    public interface IValueProvider
    {
    }

    public interface IValueProvider<out TValue> : IValueProvider
    {
        IObservable<TValue> OnProvideAsObservable();
    }
}