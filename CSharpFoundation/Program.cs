using CSharpFoundation.PracticeProblems;

namespace CSharpFoundation
{
    public class Program
    {
        static bool cont = true;
        static bool endProgram = false;
        public static void Main(string[] args)
        {
            do
            {
                Console.Write("Which program do you want to run, enter a number like:\n" +
                    "1. Check Odd or Even\n" +
                    "2. Ask Odd or Even\n" +
                    "3. Print Grade\n"
                    );
                string? inputValue = Console.ReadLine()?.ToString();
                if (string.IsNullOrEmpty(inputValue))
                {
                    do
                    {
                        Console.Write("The input is not valid !, Do you want to continue ? enter y or n: ");
                        string? continueResult = Console.ReadLine()?.ToString()??"n";
                        if (continueResult.Equals("y", StringComparison.OrdinalIgnoreCase))
                        {
                            cont = false;
                            endProgram = false;
                        }
                        else if (continueResult.Equals("n", StringComparison.OrdinalIgnoreCase))
                        {
                            cont = false;
                            endProgram = true;
                            Console.WriteLine("GoodBye !");
                            break;
                        }
                        else
                        {
                            cont = true;
                        }
                    }
                    while (cont);
                }
                else if (endProgram)
                {
                    break;
                }
                else
                {
                    int? value = string.IsNullOrEmpty(inputValue) ? 0 : int.Parse(inputValue);
                    if (value == 1)
                    {
                        CheckNum.CheckOddOrEven(2);
                        cont = false;
                    }
                    else if (value == 2)
                    {
                        CheckNum.AskEvenOrOdd();
                        cont = false;

                    }
                    else if (value == 3)
                    {
                        CheckNum.AskGrade();
                        cont = false;

                    }
                    else
                    {
                        cont = true;
                    }
                }

            } while (cont);
        }
    }
}