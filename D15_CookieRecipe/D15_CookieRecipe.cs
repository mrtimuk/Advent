using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Advent.D15_CookieRecipe
{
    class D15_CookieRecipe
    {
        List<Ingredient> Ingredients = new List<Ingredient>();

        public void Run() 
        {
            var lines = File.ReadAllLines(@"D15_CookieRecipe\D15.txt");

            var regex = new Regex(@"(\w+): capacity (-?\d+), durability (-?\d+), "+
                                  @"flavor (-?\d+), texture (-?\d+), calories (-?\d+)");

            foreach (var line in lines)
            {
                var match = regex.Match(line);
                Ingredients.Add(new Ingredient
                {
                    Name = match.Groups[1].Value,
                    Capacity = int.Parse(match.Groups[2].Value),
                    Durability = int.Parse(match.Groups[3].Value),
                    Flavor = int.Parse(match.Groups[4].Value),
                    Texture = int.Parse(match.Groups[5].Value),
                    Calories = int.Parse(match.Groups[6].Value)
                });
            }

            var portions = AportionValue(100, Ingredients.Count);
            Console.WriteLine("{0} combinations", portions.Count());

            long max = 0;
            foreach (var p in portions)
            {
                var v = Value(p);
                max = Math.Max(v, max);
            }
            Console.WriteLine("Max {0}", max);
        }

        IEnumerable<List<int>> AportionValue(int value, int n)
        {
            if (n == 1) {
                yield return new List<int> { value };
                yield break;
            }

            for (var i = 0; i <= value; i++)
            {
                var suffixes = AportionValue(value - i, n - 1);
                foreach (var suf in suffixes)
                    yield return suf.Concat(new[] { i }).ToList();
            }
        }

        class Ingredient
        {
            public string Name;
            public long Capacity;
            public long Durability;
            public long Flavor;
            public long Texture;
            public long Calories;
        }

        long Value(IEnumerable<int> weights)
        {
            var res = weights.Zip(Ingredients, (w, i) => new Ingredient
            {
                Name = i.Name,
                Capacity = w * i.Capacity,
                Durability = w * i.Durability,
                Flavor = w * i.Flavor,
                Texture = w * i.Texture,
                Calories = w * i.Calories
            }).Aggregate((i, j) => new Ingredient
            {
                Capacity = i.Capacity + j.Capacity,
                Durability = i.Durability + j.Durability,
                Flavor = i.Flavor + j.Flavor,
                Texture = i.Texture + j.Texture,
                Calories = i.Calories + j.Calories
            });

            var points = Math.Max(res.Capacity,0) * 
                Math.Max(res.Durability, 0) * 
                Math.Max(res.Flavor, 0) * 
                Math.Max(res.Texture, 0);

            return res.Calories == 500 ? points : 0;
        }
    }
}