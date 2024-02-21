using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems.SyncAsync
{




    public static class LearnSync
    {
        public static void Method1()
        {

            Console.WriteLine("Sync Method 1 is called");
        }

        public static void Method2()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Sync Method 2 is called after 2 second");
        }

        public static void Method3()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Sync Method 3 is called after 3 second");
        }
    }
    public static class LearnAsync
    {
        public static async Task Method1()
        {
            Console.WriteLine("Aync Method 1 is called");
        }

        public static async Task<int> Method2()
        {
            await Task.Delay(2000);
            Console.WriteLine("Async Method 2 is called after 2 second");

            return 1;
        }

        public static async Task Method3()
        {
            await Task.Delay(3000);
            Console.WriteLine("Async Method 3 is called after 3 second");
        }
    }
}
