using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.LearnCollections
{
    public class LearnYield
    {
        public static void Main(string[] args)
        {
            IEnumerable<int> nums = GenerateNumbers(5);
            foreach (int i in nums)
            {
                Console.WriteLine(i);
            }
        }
        public static IEnumerable<int> GenerateNumbers(int maxNum)
        {
            for (int i = 0; i < maxNum; i++)
            {
                yield return i; // everytime when i in nums of foreach is accessed, yield return is called/reached out to get the value of i
            }
        }
    }
}
