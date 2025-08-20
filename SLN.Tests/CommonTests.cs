using SLN.Utility;
using SLN.Utility.Extensions;

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

    [Theory]
    [InlineData("Director", "Director")]
    [InlineData("Director$$Manager$$Chief Executive Officer", "Director / Manager / Chief Executive Officer")]
    [InlineData("", "")]
    [InlineData(null, "")]
    public void GetRoleTest(string data1, string data2)
    {
        Assert.Equal(data2, Common.GetRole(data1));
    }

    [Fact]
    public void PositionTest()
    {
        var input = "杨军 党委书记兼主任\n吴战华 党委副书记兼纪委书记\n徐国东 党委副书记兼副主任\n娄和儒 党委委员兼二级巡视员\n任萍 党委委员兼副主任\n张锋 党委委员兼副主任\n迟泓琰 党委委员兼副主任​";
        var lines = input.ToLines();
        foreach (var line in lines)
        {
            var (name, position) = Common.SplitByLastSpace(line);
            position = Common.RemoveZeroWidthChars(position);
            var isValidPosition = Common.IsValidPostion(position);
            Assert.True(isValidPosition);
        }
    }
}