using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems.StringLearn
{
    public class StringLearn
    {
        public static void Main(string[] args)
        {
            string fruit = "apple";
            string person = null;
            string myString = null;

            Console.WriteLine($"Fruit name is {(fruit ?? "No fruit")}");
            Console.WriteLine($"Person name is {(person != null ? "No person" : "Samuel")}");
            Console.WriteLine($"Person name is {(myString?.Length ?? 0)}");
            Console.ReadLine();
        }
    }
}
