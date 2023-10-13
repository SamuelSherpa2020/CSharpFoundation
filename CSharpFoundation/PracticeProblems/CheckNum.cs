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
            int userInput = firstInput != null ? Convert.ToInt32(firstInput) : 0;

            if (userInput % 2 == 0)
            {
                Console.WriteLine("The number is even.");
            }
            else { Console.WriteLine("The number is odd."); }
        }
        #endregion

        #region Ask and Print Grade
        public static void AskGrade()
        {
            Console.Write("Enter a percentage to display student's grade:");
            string? inputValue = Console.ReadLine();
            int? percentage = string.IsNullOrEmpty(inputValue)?0:Convert.ToInt32(inputValue);
            //int? percentage = Convert.ToInt32(Console.ReadLine()?.ToString() ?? "0");
            switch (percentage / 10)
            {
                case 10:
                    Console.WriteLine("The grade of a student is \"A+\".");
                    break;
                case 9:
                    Console.WriteLine("The grade of a student is A+");
                    break;
                case 8:
                    Console.WriteLine("The grade of a student is 'A'.");
                    break;
                case 7:
                    Console.WriteLine("The grade of a student is B+");
                    break;
                case 6:
                    Console.WriteLine("The grade of a student is B");
                    break;
                case 5:
                    Console.WriteLine("The grade of a student is C+");
                    break;
                case 4:
                    Console.WriteLine("The grade of a student is C+");
                    break;
                case 3:
                    Console.WriteLine("The grade of a student is D");
                    break;

                default:
                    Console.WriteLine("You are failed.");
                    break;
            }
        }
        #endregion

    }
}
