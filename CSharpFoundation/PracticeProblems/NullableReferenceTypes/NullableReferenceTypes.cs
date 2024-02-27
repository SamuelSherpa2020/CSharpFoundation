using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFoundation.PracticeProblems.NullableReferenceTypes
{
    public class NullableReferenceTypes
    {
        public static void Main(string[] args)
        {
            string? Message = null;


            if(!string.IsNullOrEmpty(Message))
            {
            Console.WriteLine($"The length of the Message is {Message.Length}\n");
            }
            //Console.WriteLine($"The length of the Message is {Message.Length}\n");

            var originalMessage = Message;

            Message = "Hello, World";
            Console.WriteLine($"The length of the Message is {Message}");

            Console.WriteLine($"The length of the originalMessage is: {originalMessage.Length}");
        }
    }
}
