using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
    }

}
