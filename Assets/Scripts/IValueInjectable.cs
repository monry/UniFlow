using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace UniFlow
{
    public interface IValueInjectable<TValue>
    {
        TValue Value { get; set; }
//        void Inject(TValue value);
//        IObservable<Unit> WaitForInjectAsObservable();
    }

    public static class ValueInjectableExtensions
    {
        public static void Inject<TValue>(this IValueInjectable<TValue> injectable, TValue value)
        {
            injectable.Value = value;
        }

        public static void InjectAll<TValue>(this IEnumerable<IValueInjectable<TValue>> injectables, TValue value)
        {
            injectables.ToList().ForEach(x => x.Inject(value));
        }
    }
}
