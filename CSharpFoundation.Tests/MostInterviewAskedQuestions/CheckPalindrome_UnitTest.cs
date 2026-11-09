using CSharpFoundation.MostInterviewAskedQuestions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpFoundation.Tests.MostInterviewAskedQuestions
{
    public class CheckPalindromeUnitTest
    {
        [Fact]
        public void TestPalindromeChecker()
        {
            // Arrange
            var palindromeWord = "racecar";
            var expected = true;
            // Act
            var actual = CheckPalindrome.PalindromeChecker(palindromeWord);
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
