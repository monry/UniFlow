using System.Collections.Generic;
using System.Linq;

namespace UniFlow
{
    public interface IInjectable<in TValue>
    {
        void Inject(TValue value);
    }

    public static class InjectableExtensions
    {
        public static void InjectAll<TValue>(this IEnumerable<IInjectable<TValue>> injectables, TValue value)
        {
            injectables.ToList().ForEach(x => x.Inject(value));
        }
    }
}
