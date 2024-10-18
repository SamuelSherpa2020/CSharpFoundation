using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems
{
    public class ReverseGivenWords
    {
        public static void Main(string[] args)
        {
            Console.Write("Enter your sentence: ");
            string? givenSentence = Console.ReadLine();
            string[] givenWords = givenSentence!.Split(' ');
            string reversedWords = string.Empty;
            
            for (int i = givenWords.Length - 1; i >= 0; i--)
            {
                reversedWords  += givenWords[i] +  " ";
            }
            Console.WriteLine(reversedWords);
        }
    }
}
