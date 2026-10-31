using CSharpFoundation.MostInterviewAskedQuestions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpFoundation.Tests.MostInterviewAskedQuestions
{
    public class CheckPalindrome_UnitTest
    {
        [Fact]
        public void TestPalindromeChecker()
        {
            // Arrange
            string palindromeWord = "racecar";
            bool expected = true;
            // Act
            bool actual = CheckPalindrome.PalindromeChecker(palindromeWord);
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
