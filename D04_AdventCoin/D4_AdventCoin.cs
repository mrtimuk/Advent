using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Advent.D04_AdventCoin
{
    class D4_AdventCoin
    {
        public void Run() 
        {
            var md5 = MD5.Create();

            int i = 0;
            do
            {
                var input = "ckczppom" + i;

                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < hash.Length; j++)
                    sb.Append(hash[j].ToString("X2"));

                if (sb.ToString().StartsWith("000000"))
                {
                    Console.WriteLine("Hash: {0},  Number: {1}", sb, i);
                    break;
                }
                i++;
            } while (true);
        }
    }
}
