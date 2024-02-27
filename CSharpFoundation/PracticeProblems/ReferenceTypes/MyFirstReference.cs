using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems.ReferenceTypes
{
    public class MyFirstReference
    {
        public string? Myname { get; set; }
        public int MyAge { get; set; }
    }


    public class Program
    {
        public static void Main(string[] args)
        {
            MyFirstReference obj1 = new MyFirstReference();
            obj1.Myname = "Samuel";

            MyFirstReference obj2 = obj1;
            obj2.Myname = "MJ";

            Console.WriteLine($"My name is: {obj2.Myname}");
        }

    }
}
