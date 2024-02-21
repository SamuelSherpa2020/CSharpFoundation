﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems.SyncAsync
{
    public class LearnAsyncUsingGPT
    {
        public static void CallSyncMethod()
        {
            Console.WriteLine("The Sync method Opened");
            //for (int i = 0; i < 100000000; i++)
            //{

            //}
            Thread.Sleep(1000);
            Console.WriteLine("The Sync method Closed");
        }
        public static async Task CallAsyncMethod()
        {
            Console.WriteLine("The Async program Opened");
            await Task.Delay(1000);
            await Console.Out.WriteLineAsync("The Async program Closed");
        }
    }

}
