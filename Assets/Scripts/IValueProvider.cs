using System;

namespace UniFlow
{
    public interface IValueProvider<out TValue>
    {
        IObservable<TValue> OnProvideAsObservable();
    }
}