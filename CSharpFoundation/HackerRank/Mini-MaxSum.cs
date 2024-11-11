using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.HackerRank
{
    public class Mini_MaxSum
    {
        public static void Main(string[] args)
        {
            List<int> arr = new List<int> { 1, 2, 3, 4, 5 };
            //arr.Sort();

            int arrLength = arr.Count;
            long minMaxNum = 0;

            List<long> arr2 = new List<long>(arrLength);

           long[] newArr = new long[arrLength];

            int index = 0;
            foreach (int item in arr)
            {
                newArr[index] = item;
                index++;
            }

            int bigWhileCounter = arrLength;
            int counter = 0;

            while (bigWhileCounter != 0)
            {
                for (int i = 0; i < arrLength; ++i)
                {
                    while (counter != i)
                    {
                        minMaxNum += newArr[i];
                        break;
                    }
                }

                counter++;
                arr2.Add(minMaxNum);
                bigWhileCounter--;
                minMaxNum = 0;
            }

            Console.WriteLine(arr2.Min() + " " + arr2.Max());
            Console.ReadLine();
        }
    }
}


