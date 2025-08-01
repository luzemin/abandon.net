using SLN.Utility;

namespace SLN.Tests;

public class CommonTests
{
    [Theory]
    [InlineData("(2012)黄浦民一(民)初字第833号", "（2012）黄浦民一（民）初字第833号")]
    [InlineData("(2012)黄浦民一(民)初字第833号", "(2012)黄浦民一（民）初字第833号")]
    [InlineData("(2012)黄浦民一(民)初字第833号", "（2012）黄浦民一(民)初字第833号")]
    
    [InlineData("(2012)黄浦民一(民)初字第833号", "2012黄浦民一(民)初字第833号")]
    [InlineData("(2012)黄浦民一(民)初字第833号", "(2012)黄浦民一民初字第833号")]
    [InlineData("(2012)黄浦民一(民)初字第833号", "2012黄浦民一民初字第833号")]
    
    [InlineData("(2012)黄浦民一(民)初字第833号", "(2012)黄浦民一(民)初第833号")]
    
    [InlineData("(2012)黄浦民一(民)初字第833号", "(2012)黄浦民一(民)初833号")]
    
    [InlineData("(2012)黄浦民一(民)初字第833号", "(2012)黄浦民一(民)初0833号")]
    [InlineData("(2012)黄浦民一(民)初字第833号", "(2012)黄浦民一(民)初00833号")]
    public void IsSameCaseNumberTest(string data1, string data2)
    {
        Assert.True(Common.IsSameCaseNumber(data1, data2));
    }
}