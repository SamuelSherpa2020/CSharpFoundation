using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems
{
    public class LearnAboutNull
    {
        public static void NullToInt()
        {
            object value = null;
            try
            {
                int result = Convert.ToInt32(value);
                //string result = value.ToString();
                Console.WriteLine("The conversion was successful !"+result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Conversion Failed ! Exception: "+ex.Message);
            }            
        }

        public static void NullToString()
        {
            string value = null; // this is used, and exception will be thrown
            string value2 = "";
            try
            {
                string result = Convert.ToString(value); // Convert.ToString() will convert null into => ""
                //where as value.ToString() will throw exception
                Console.WriteLine("The conversion was successful !"+result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("The conversion threw exception: "+ex.Message);
                throw;
            }
        }
    }
}
