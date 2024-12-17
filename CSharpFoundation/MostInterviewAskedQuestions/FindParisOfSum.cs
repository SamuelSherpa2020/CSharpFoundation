namespace CSharpFoundation.MostInterviewAskedQuestions
{
    internal class FindParisOfSum
    {
        public static void Main()
        {
            int[] nums = { 1, 2, 6, 7, 8, 9, 3, 4, 5, -10, 20 };
            int targetSum = 10;
            var pairs = FindPairs(nums, targetSum);
            foreach (var pair in pairs)
            {
                Console.WriteLine(pair);
            }
            Console.ReadLine();
        }
        public static List<(int, int)> FindPairs(int[] arr, int targetSum)
        {
            List<(int, int)> pairs = new List<(int, int)>();
            HashSet<int> visited = new HashSet<int>();
            foreach (var item in arr)
            {
                int complement = targetSum - item;
                if (visited.Contains(item))
                {
                    pairs.Add((complement, item));
                }
                visited.Add(complement);
            }
            return pairs;
        }
    }
}
