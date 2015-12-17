using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public static class Utils
    {
        public static T GetOrAdd<T>(this Dictionary<string, T> dict, string key) where T : new()
        {
            T obj;
            if (!dict.TryGetValue(key, out obj))
            {
                obj = new T();
                dict.Add(key, obj);
            }
            return obj;
        }

        public static IEnumerable<T[]> Permutations<T>(IEnumerable<T> places)
            where T : IComparable
        {
            if (places.Count() == 1)
            {
                yield return new[] { places.First() };
                yield break;
            }
            foreach (var place in places)
            {
                var suffixes = Permutations(places.Where(p => p.CompareTo(place) != 0));
                foreach (var suffix in suffixes)
                    yield return new[] { place }.Concat(suffix).ToArray();
            }
        }
    }
}
