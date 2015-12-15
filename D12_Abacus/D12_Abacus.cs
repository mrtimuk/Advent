using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Advent
{
    public class D12_Abacus
    {
        public void Run()
        {
            var txt = File.ReadAllText("D12.txt");
            var token = JObject.Parse(txt);

            Console.WriteLine("Sum: " + Sum(token));
        }

        int Sum(JToken tok)
        {
            int sum = 0;

            switch(tok.Type)
            {
                case JTokenType.Property:
                    var prop = (JProperty)tok;
                    sum += Sum(prop.Value);
                    break;

                case JTokenType.Float:
                case JTokenType.Integer:
                    sum += tok.Value<int>();
                    break;

                case JTokenType.Array:
                    foreach (var child in tok.Children())
                        sum += Sum(child);
                    break;

                case JTokenType.Object:
                    var red = tok.Children()
                        .OfType<JProperty>()
                        .Any(p => p.Value.Type == JTokenType.String &&
                            p.Value.Value<string>().Equals("red", StringComparison.InvariantCultureIgnoreCase));

                    if (!red) {
                        foreach (var child in tok.Children()) sum += Sum(child);
                    }
                    break;
            }
            return sum;
        }
    }
}
