using Advent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Advent.D13_DinnerTable
{
    class D13_DinnerTable
    {
        Dictionary<string, Dictionary<string, int>> Moods = 
            new Dictionary<string, Dictionary<string, int>>();

        public void Run()
        {
            var lines = File.ReadAllLines(@"D13_DinnerTable\D13.txt");

            var rLine = new Regex(
                @"(\w+) would (gain|lose) (\d+) happiness units by sitting next to (\w+).");

            foreach(var line in lines)
            {
                var m = rLine.Match(line);
                var person = m.Groups[1].Value;
                var gain = m.Groups[2].Value == "gain" ? 1 : -1;
                var mood = int.Parse(m.Groups[3].Value) * gain;
                var other = m.Groups[4].Value;

                Moods.GetOrAdd(person).Add(other, mood);
            }

            var persons = Moods.Keys.ToList();
            foreach(var person in persons)
            {
                Moods.GetOrAdd("Me").Add(person, 0);
                Moods.GetOrAdd(person).Add("Me", 0);
            }

            var permutations = Utils.Permutations(Moods.Keys);

            var maxHappiness = permutations.Select(perm => Score(perm)).Max();
            Console.WriteLine("Max hapiness {0}", maxHappiness);
        }

        int Score(string [] permutation)
        {
            var total = 0;
            for (int i = 0; i < permutation.Length; i++)
            {
                var left = i == 0 ? permutation.Length - 1 : i - 1;
                var right = i == permutation.Length - 1 ? 0 : i + 1;

                total += Moods[permutation[i]][permutation[left]];
                total += Moods[permutation[i]][permutation[right]];
            }
            return total;
        }
    }
}