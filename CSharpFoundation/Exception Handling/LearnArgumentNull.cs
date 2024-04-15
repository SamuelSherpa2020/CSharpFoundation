using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.Exception_Handling
{
    public class LearnArgumentNull
    {
        public static void Main(string[] args)
        {
            GiveName("");
            Console.ReadLine();
        }

        public static void GiveName(string chilling)
        {
            try
            {
                if (string.IsNullOrEmpty(chilling))
                {
                    throw new ArgumentException(nameof(chilling));
                }
                else
                {
                    Console.WriteLine(chilling);
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch
            {
                Console.WriteLine("An unexpected error occurred.");
            }
        }
    }
}
