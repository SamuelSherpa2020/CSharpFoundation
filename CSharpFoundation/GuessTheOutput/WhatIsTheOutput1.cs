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
            int J = 0;
            for (int I = 2; I <= 10; I +=2)
            {
                J += I;
            }
            Console.WriteLine("The total sum is: {0}",J);
            Console.ReadLine();
        }
    }
}
