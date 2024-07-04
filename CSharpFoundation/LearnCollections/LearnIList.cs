
namespace CSharpFoundation.LearnCollections;

public class LearnIList
{
    public static void Main(string[] args)
    {
        IList<string> continents = new List<string>() { "Asia", "Australia", "SouthAmerica", "NorthAmerica", "Europe", "Africa", "Antartica" };
        Console.WriteLine(continents[0]);
        foreach (string continent in continents)
            Console.Write(continent + " ");

        Console.WriteLine();

        continents.Remove("Antartica");
        foreach (string continent in continents)
            Console.Write(continent + " ");


        Console.WriteLine();

        continents.RemoveAt(0);
        foreach (string continent in continents)
            Console.Write(continent + " ");

        continents.Add("Antartica");
        continents.Insert(0,"Asia");
        foreach (string continent in continents)
            Console.Write(continent + " ");
    }
}
