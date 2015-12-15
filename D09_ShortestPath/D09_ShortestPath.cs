using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent.D09_ShortestPath
{
    class D09_ShortestPath
    {
        public void Run()
        {
            Load();

            var routes = Routes(places);
            int min = int.MaxValue;
            foreach (var route in routes)
            {
                var dist = 0;
                for (int i = 0; i < route.Length - 1; i++)
                {
                    var a = route[i];
                    var b = route[i + 1];
                    if (string.Compare(a, b) > 0) { var t = a; a = b; b = t; }

                    dist += distances[a][b];
                }
                min = Math.Min(min, dist);
                Console.WriteLine(string.Join(" -> ", route) + "  " + dist);
            }

            Console.WriteLine("Min dist is " + min);
        }

        List<string> places = new List<string>();

        Dictionary<string, Dictionary<string, int>> distances =
            new Dictionary<string, Dictionary<string, int>>();

        IEnumerable<string[]> Routes(IEnumerable<string> places)
        {
            if (places.Count() == 1)
            {
                yield return new[] { places.First() };
                yield break;
            }
            foreach (var place in places)
            {
                var suffixes = Routes(places.Where(p => p != place));
                foreach (var suffix in suffixes)
                    yield return new[] { place }.Concat(suffix).ToArray();
            }
        }

        void Load()
        {
            var rRoute = new Regex(@"(\w+) to (\w+) = (\d+)");
            var input = File.ReadAllLines("input.txt");
            var myplaces = new HashSet<string>();
            foreach (var line in input)
            {
                var mRoute = rRoute.Match(line);
                var a = mRoute.Groups[1].Value;
                var b = mRoute.Groups[2].Value;
                var dist = int.Parse(mRoute.Groups[3].Value);

                myplaces.Add(a);
                myplaces.Add(b);

                if (string.Compare(a, b) > 0) { var t = a; a = b; b = t; }

                Dictionary<string, int> distancesFromA;
                if (!distances.TryGetValue(a, out distancesFromA))
                {
                    distancesFromA = new Dictionary<string, int>();
                    distances.Add(a, distancesFromA);
                }
                distancesFromA.Add(b, dist);
            }
            places = myplaces.ToList();
        }
    }
}
