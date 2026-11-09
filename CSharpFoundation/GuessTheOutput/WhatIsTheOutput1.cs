using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.GuessTheOutput
{
    public class WhatIsTheOutput1
    {
        public static void Main(string[] args)
        {
            var j = 0;
            for (var I = 2; I <= 10; I +=2)
            {
                j += I;
            }
            Console.WriteLine("The total sum is: {0}",j);
            Console.ReadLine();
        }
    }
}
