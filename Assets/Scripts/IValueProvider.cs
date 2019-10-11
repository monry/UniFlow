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
}
