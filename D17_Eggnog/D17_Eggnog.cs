using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Advent.D17_Eggnog
{
    class D17_Eggnog
    {
        void Run()
        {
            var lines = File.ReadAllLines(@"D17_Eggnog\D17.txt");
            var capacities = lines.Select(l => int.Parse(l)).ToList();

            var total = 150;
            var combs = Combs(total, capacities);
            var lengths = combs.Select(l => l.Count()).OrderBy(i => i);
            var min = lengths.First();

            var count = lengths.TakeWhile(i => i == min).Count();
            Console.WriteLine("Combinations: {0}", count);
        }

        IEnumerable<List<int>> Combs(int qty, List<int> capacities)
        {
            var cap = capacities.First();

            if (capacities.Count == 1)
            {
                if (qty == cap) yield return new List<int> { cap };
                if (qty == 0) yield return new List<int> { };
                yield break;
            }

            var others = capacities.Skip(1).ToList();

            foreach (var comb in Combs(qty, others))
                yield return comb;

            if (qty >= cap)
                foreach (var comb in Combs(qty - cap, others))
                    yield return new[] { cap }.Concat(comb).ToList();
        }
    }
}