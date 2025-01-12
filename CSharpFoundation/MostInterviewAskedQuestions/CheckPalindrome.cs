namespace CSharpFoundation.MostInterviewAskedQuestions
{
    internal class CheckPalindrome
    {
        public static void Main()
        {
            Console.WriteLine("Enter the word to check if it's palindrome:");
            var palindromeWord = Console.ReadLine();
            bool result = PalindromeChecker(palindromeWord!);
            if (result)
                Console.WriteLine($"The {palindromeWord} is a palindrome");
            else
                Console.WriteLine($"The {palindromeWord} is not a palindrome");
        }

        public static bool PalindromeChecker(string palindromeWord)
        {
            palindromeWord.ToCharArray();
            int left = 0, right = palindromeWord.Length - 1;
            while (left < right)
            {
                if (palindromeWord[left] != palindromeWord[right])
                {
                    return false;
                }
                left++;
                right--;
            }
            return true;
        }
    }
}
