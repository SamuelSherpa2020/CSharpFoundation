using CSharpFoundation.PracticeProblems;
using System.Text.RegularExpressions;

namespace CSharpFoundation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string? inputValue;
            int number;
            do
            {
                Console.WriteLine("X-----------------X");
                Console.Write("Which program do you want to run, only number like:1,2,3.. is allowed:-\n" +
                    "1. Check Odd or Even\n" +
                    "2. Ask Odd or Even\n" +
                    "3. Print Grade\n" +
                    "4. Check NullToInt\n" +
                    "5. Check NullToString\n" +
                    "6. Print Value Greater than 80 using LINQ\n" +
                    "7. Call Value Async and sync method at same time\n"
                    );
                inputValue = Console.ReadLine()?.ToString() ?? "Null";

                #region Long Code to Check User Input

                //if (string.IsNullOrEmpty(inputValue))
                //{
                //    do
                //    {
                //        Console.Write("The input is not valid !, Do you want to continue ? enter y or n: ");
                //        string? continueResult = Console.ReadLine()?.ToString()??"n";
                //        if (continueResult.Equals("y", StringComparison.OrdinalIgnoreCase))
                //        {
                //            cont = false;
                //            endProgram = false;
                //        }
                //        else if (continueResult.Equals("n", StringComparison.OrdinalIgnoreCase))
                //        {
                //            cont = false;
                //            endProgram = true;
                //            Console.WriteLine("GoodBye !");
                //            break;
                //        }
                //        else
                //        {
                //            cont = true;
                //        }
                //    }
                //    while (cont);
                //}
                //else if (endProgram)
                //{
                //    break;
                //}
                #endregion

                int? value = string.IsNullOrEmpty(inputValue) ? 0 : int.Parse(inputValue);

                RunRequestedProgram(value);

            } while (!IsNumeric(inputValue, out number));
        }

        static bool IsNumeric(string inputValue, out int number)
        {
            //use a regex to check and allow only numeric input
            if (Regex.IsMatch(inputValue, @"^\d+$"))
            {
                number = int.Parse(inputValue);
                return true;
            }
            else
            {
                Console.Write("The input is not valid !, Do you want to continue ? enter y or n: ");
                string? continueResult = Console.ReadLine()?.ToString() ?? "n";
                if (continueResult.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    number = 0;
                    return false;
                }
                else if (continueResult.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("GoodBye ! The program has ended.");
                    Console.WriteLine("X-----------------X");

                    number = 0;
                    return true;
                }
                else
                {
                    Console.WriteLine("The program will be continuing:");
                    number = 0;
                    return false;
                }

            }
        }

        static async void RunRequestedProgram(int? value)
        {
            if (value == 1)
            {
                CheckNum.CheckOddOrEven(2);
                Console.WriteLine("X-----------------X");

            }
            else if (value == 2)
            {
                CheckNum.AskEvenOrOdd();
                Console.WriteLine("X-----------------X");

            }
            else if (value == 3)
            {
                CheckNum.AskGrade();
                Console.WriteLine("X-----------------X");

            }
            else if (value == 4)
            {
                LearnAboutNull.NullToInt();
                Console.WriteLine("X----------------X");
            }
            else if (value == 5)
            {
                LearnAboutNull.NullToString();
                Console.WriteLine("X----------------X");
            }
            else if (value == 6)
            {
                LearnLINQ.PrintNumGreaterThan80();
                Console.WriteLine("X----------------X");
            }
            else if (value == 7)
            {
                LearnSync.Method1();
                LearnSync.Method2();
                LearnSync.Method3();


               int var =  await LearnAsync.Method2();
                await LearnAsync.Method1();
                await LearnAsync.Method3();

                //below for synchronous coding

                Console.WriteLine("X----------------X");
            }
            else
            {
                Console.WriteLine("The input entered performs nothing.");
            }
        }
    }
}