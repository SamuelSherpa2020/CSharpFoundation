

using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace CSharpFoundation.LearnCollections;

public class LearnICollection
{
    public static void Main()
    {
        ICollection<string> countries = new Collection<string>();
        ICollection<string> states = new List<string>();
        
        //Adding Countries
        countries.Add("Nepal");
        countries.Add("Russia");
        countries.Add("USA");
        foreach (string country in countries)
            Console.WriteLine(country);
       
        Console.WriteLine(countries.Count);
        Console.WriteLine();
        
        countries.Remove("Russia");
        foreach (string country in countries)
            Console.WriteLine(country);
        countries.Clear();
        
        Console.WriteLine("After Clearing Countries");

        foreach (string country in countries)
            Console.WriteLine(country);


        //Adding States
        states.Add("Koshi");
        states.Add("Amur");
        states.Add("Colorado");

        foreach (string state in states)
            Console.WriteLine(state);
    }
}
