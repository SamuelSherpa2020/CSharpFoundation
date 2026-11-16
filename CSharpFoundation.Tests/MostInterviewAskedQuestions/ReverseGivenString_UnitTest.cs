using CSharpFoundation.PracticeProblems;

namespace CSharpFoundation.Tests.MostInterviewAskedQuestions;

public class ReverseGivenString_UnitTest
{

    // Below is for testing different scenarios
    [Theory]
    [InlineData("Hello World!", "!dlroW olleH")]
    [InlineData("", "")]
    [InlineData("abcd", "dcba")]
    [InlineData("a", "a")]
    [InlineData("madam", "madam")]
    public void ReverseString_ReturnsReversed(string stringToReverse, string reversedString)
    {
        string? result = ReverseGivenString.ReverseString(stringToReverse);
        Assert.Equal(reversedString, result);
    }
    
    // Below is for testing the Null
    [Fact]
    public void ReverseString_NullInput_ReturnsNull()
    {
        string? result = ReverseGivenString.ReverseString(null);
        Assert.Null(result);
    }
}