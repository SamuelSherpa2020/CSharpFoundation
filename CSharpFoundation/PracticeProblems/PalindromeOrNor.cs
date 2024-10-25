using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems
{
    public class PalindromeOrNor
    {
        static void Main(string[] args)
        {
            Console.Write("Only string is allowed: ");
            var input1 = Console.ReadLine()!.ToString().ToLower();
            char[] input2 = input1.ToCharArray();
            string finalWord = string.Empty;

            for (int i = input2.Length - 1; i >= 0; i--)
            {
                finalWord += input2[i];
            }
            if (finalWord.Equals(input1))
            {
                Console.WriteLine($"The given word {finalWord} is a palindrome");
            }
            else
            {
                Console.WriteLine($"The given word {finalWord} is a not a palindrome");
            }
        }
    }
}
