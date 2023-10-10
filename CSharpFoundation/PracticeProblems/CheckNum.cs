using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems
{
    public class CheckNum
    {

        #region CheckOddOrEven
        public static void CheckOddOrEven(int num)
        {
            if (num % 2 == 0)
            {
                Console.WriteLine("The number is Even");

            }
            else
            {
                Console.WriteLine("The number is Odd");
            }
        }
        #endregion

        #region Ask and print EvenOrOdd
        public static void AskEvenOrOdd()
        {
            //string? firstInput = Console.ReadLine()?.ToString()??"0";
            //int userInput = Convert.ToInt32(firstInput);
            Console.Write("Enter a number to check odd or even:");
            int? firstInput = Convert.ToInt32(Console.ReadLine());
            int userInput = firstInput != null?Convert.ToInt32(firstInput) : 0;
            if(userInput %2 == 0)
            {
                Console.WriteLine("The number is even.");
            }
            else { Console.WriteLine("The number is odd."); }
        }
        #endregion
    }
}
