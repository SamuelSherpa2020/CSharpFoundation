using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonName
{
    public class JoinWords
    {
        public static void IJoinJummbleWords()
        {
            string firstName = "Arunbbb";
            string secondName = "book";
            int totalWordLength = CountWords(firstName) + CountWords(secondName);

            char[] letterHolder = new char[totalWordLength];
            char[] firstNameArray = firstName.ToCharArray();
            char[] secondNameArray = firstName.ToCharArray();

            int Counter1 = 0;
            for (int i = 0; i < firstNameArray.Length;i++)
            {
                char temp = firstName[i];
                letterHolder[Counter1] = temp;
                Counter1 += 2;
            }

            int Counter2 = 1;
            for (int i = 0; i < secondName.Length; i++)
            {
                char temp = secondNameArray[i];
                letterHolder[Counter2] = temp;
                Counter2 += 2;

            }
            string WholeWord = string.Join("", letterHolder);
            Console.WriteLine(WholeWord);
            //Console.WriteLine("Hi I am Samuel");
            //Console.ReadLine();
        }
        public static int CountWords(string input)
        {
            int count = 0;

            foreach (char c in input)
            {
                // Check if the character is a letter (using char.IsLetter)
                if (char.IsLetter(c))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
