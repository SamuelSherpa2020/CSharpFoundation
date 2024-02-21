using System;
using System.Collections.Generic;
using System.Numerics;

//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems.Linq
{
    public class LearnLINQ
    {
        public static void PrintNumGreaterThan80()
        {
            int[] scores = { 1, 56, 78, 89, 98, 101 };
            IEnumerable<int> scoreQuery = from score in scores
                                          where score > 80
                                          select score;

            IEnumerable<string> secondQuery = from score in scores
                                              where score > 80
                                              orderby score descending
                                              select $"The number is {score}";

            foreach (var item in secondQuery)
            {
                Console.WriteLine("The number are persented to you in descending order:\n" + item + "\n");
            }
        }
    }
}
