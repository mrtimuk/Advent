using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent.D14_ReindeerRace
{
    class D14_ReindeerRace
    {
        public void Run()
        {
            var defs = File.ReadAllLines(@"D14_ReindeerRace\D14.txt");

            var reg = new Regex(@"(\w+) can fly (\d+) km/s for (\d+) seconds, but then must rest for (\d+) seconds");

            var reindeer = new List<Reindeer>();
            foreach (var def in defs)
            {
                var match = reg.Match(def);

                reindeer.Add(new Reindeer
                {
                    Name = match.Groups[1].Value,
                    Speed = int.Parse(match.Groups[2].Value),
                    Endurance = int.Parse(match.Groups[3].Value),
                    Rest = int.Parse(match.Groups[4].Value),
                });
            }

            var scores = new int[reindeer.Count];
            for (int t = 1; t <= 2503; t++)
            {
                var rs = reindeer.Zip(reindeer.Select(r => Distance(r, t)), 
                                      (r, d) => Tuple.Create(r, d))
                                 .OrderByDescending(kvp => kvp.Item2);

                var winner = rs.First();
                foreach (var r in rs.TakeWhile(rS => rS.Item2 == winner.Item2))
                    r.Item1.Score++;
            }

            foreach (var r in reindeer.OrderBy(r => r.Score))
                Console.WriteLine("{0} scores {1}", r.Name, r.Score);

            var finalWinner = reindeer.OrderBy(r => r.Score).Last();
            Console.WriteLine("{0} wins, {1}", finalWinner.Name, finalWinner.Score);
        }

        int Distance(Reindeer reindeer, int time)
        {
            int cycle = reindeer.Endurance + reindeer.Rest;
            int cycles = time / cycle;
            int residue = Math.Min(time % cycle, reindeer.Endurance);
            int timeFlying = cycles * reindeer.Endurance + residue;
            int distance = timeFlying * reindeer.Speed;

            return distance;
        }
    }

    class Reindeer {
        public int Score;
        public string Name;
        public int Speed;
        public int Endurance;
        public int Rest;
    }
}
