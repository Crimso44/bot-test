using System;
using System.Collections.Generic;

namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    abstract class LazyInitSupport
    {
        protected static void LazyInit<T>(ref T target, Func<T> source)
        {
            if (target != null)
                return;

            target = source();
        }

        protected static void LazyInit<T>(ref ISet<T> target, Func<IEnumerable<ISet<T>>> sources)
        {
            if (target != null)
                return;

            target = new HashSet<T>();

            foreach (var set in sources())
                target.UnionWith(set);
        }
    }
}
