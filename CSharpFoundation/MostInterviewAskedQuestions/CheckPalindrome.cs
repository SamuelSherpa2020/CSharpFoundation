namespace CSharpFoundation.MostInterviewAskedQuestions
{
    public static class CheckPalindrome
    {
        public static void Main()
        {
            Console.WriteLine("Enter the word to check if it's palindrome:");
            var palindromeWord = Console.ReadLine();
            bool result = PalindromeChecker(palindromeWord!);
            Console.WriteLine(result
                ? $"The {palindromeWord} is a palindrome"
                : $"The {palindromeWord} is not a palindrome");
        }

        public static bool PalindromeChecker(string palindromeWord)
        {
            //palindromeWord.ToCharArray();
            short left = 0;
            short right = (short)(palindromeWord.Length - 1);
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
