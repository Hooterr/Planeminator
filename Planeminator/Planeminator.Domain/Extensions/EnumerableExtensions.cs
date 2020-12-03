using System;
using System.Collections.Generic;
using System.Text;

namespace Planeminator.Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach(var item in source)
            {
                action(item);
            }
            return source;
        }
    }

}
