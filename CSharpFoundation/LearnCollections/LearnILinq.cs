

namespace CSharpFoundation.LearnCollections;

public class FamilyMembers
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int? Age { get; set; }
}
public class LearnILinq
{
    public static void Main(string[] args)
    {
        IQueryable<int> query = (new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }).AsQueryable();
        var evenNumbers = query.Where(x => x % 2 == 0);

        foreach (var item in evenNumbers)
        {
            Console.Write(item + " ");
        }

        Console.WriteLine("X----------X");
        IQueryable<FamilyMembers> students = (new List<FamilyMembers>()
        {
            new FamilyMembers{Id=1,Name="Dawa Norbu Sherpa",Age=48},
            new FamilyMembers{Id=2,Name="Rojy Karki Sherpa",Age=43},
            new FamilyMembers{Id=3,Name="Subhekchya Sherpa",Age = 24},
            new FamilyMembers{Id=4,Name="Samuel Sherpa",Age = 16},
        }).AsQueryable();

        var selectedMembers = students.Where(x => x.Age > 20);
        foreach (var item in selectedMembers)
        {
            Console.WriteLine(item.Id + " " + item.Name + " " + item.Age);
        }
    }
}
