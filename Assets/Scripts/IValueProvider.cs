using System;
using System.Collections.Generic;

namespace UniFlow
{
    public interface IValueProvider<out TValue>
    {
        TValue Provide();
    }

    public interface IListValueProvider<out TValue>
    {
        IEnumerable<TValue> Provide();
    }

    public interface IFilteredListValueProvider<TValue> : IListValueProvider<TValue>
    {
        Func<TValue, bool> Predicate { get; }
    }
}
