using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems.ExtensionMethods
{
     static class SecondGeekClass
    {
        public static void M4(this Geek g)
        {
            Console.WriteLine("Method name: M4");
        }
        public static void M5(this Geek g, string str)
        {
            Console.WriteLine($"The given parameter string is: {str}");
        }
    }
}
