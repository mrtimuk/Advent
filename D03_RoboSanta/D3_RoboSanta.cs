using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent.D03_RoboSanta
{
    class D3_RoboSanta
    {
        public void Run()
        {
            string d = File.ReadAllText("d3.txt");

            var map = new HashSet<long>();
            map.Add(0);

            int x1 = 0, y1 = 0;
            int x2 = 0, y2 = 0;
            bool rob = false;

            foreach (var c in d)
            {
                if (rob)
                {
                    switch (c)
                    {
                        case '>': x2++; break;
                        case '<': x2--; break;
                        case '^': y2++; break;
                        case 'v': y2--; break;
                    }
                    map.Add(x2 * 1000000 + y2);
                }
                else
                {
                    switch (c)
                    {
                        case '>': x1++; break;
                        case '<': x1--; break;
                        case '^': y1++; break;
                        case 'v': y1--; break;
                    }
                    map.Add(x1 * 1000000 + y1);
                }
                rob = !rob;
            }
            Console.WriteLine("{0} houses", map.Count());
        }
    }
}
