using System.Collections;


namespace CSharpFoundation.LearnCollections;

public class LearnEnumerable
{
    public static void Main(string[] args)
    {
        var cities = new List<string>() { "New York", "London", "Tokyo", "Lisbon", "Hyderabad", "Chicago" };

        IEnumerable<string> query = cities.Where(x => x.StartsWith("L"));

        IEnumerator<string> getEnumerator = query.GetEnumerator();
        while (getEnumerator.MoveNext())
        {
            string city = getEnumerator.Current;
            Console.WriteLine(city);
        }
        foreach (var c in query)
            Console.WriteLine(c);
    }
}
