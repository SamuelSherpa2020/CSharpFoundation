using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems
{
    public class ReverseGivenString
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter the string: ");
            string stringToReverse = Console.ReadLine()!;
            char[] words = stringToReverse.ToCharArray();
            string reversedString = string.Empty;

            for (int i = words.Length-1; i >= 0; i--)
            {
                reversedString += words[i];
            }
            Console.WriteLine($"The reverse of the string is :{reversedString}");
        }
    }
}
