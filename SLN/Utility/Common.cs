using System.Text.RegularExpressions;
using SLN.Utility.Extensions;

namespace SLN.Utility;

public static class Common
{
    private static readonly Regex CaseNumberRegex = new Regex("字?第?0*(\\d*)号", RegexOptions.Compiled);

    public static bool IsSameCaseNumber(string caseNo1, string caseNo2)
    {
        if (string.IsNullOrWhiteSpace(caseNo1) || string.IsNullOrWhiteSpace(caseNo2)) return false;
        if (caseNo1 == caseNo2) return true;

        caseNo1 = CaseNumberRegex.Replace(NormalizeCaseNumber(caseNo1), "${1}号");
        caseNo2 = CaseNumberRegex.Replace(NormalizeCaseNumber(caseNo2), "${1}号");

        return caseNo1 == caseNo2;
    }

    private static string NormalizeCaseNumber(string input)
    {
        return input.Replace("(", "").Replace(")", "").Replace("（", "").Replace("）", "").Replace(" ", "");
    }

    public static string GetRole(string role)
    {
        if (string.IsNullOrEmpty(role)) return string.Empty;
        if (role.Contains("$$"))
        {
            role = role.Replace("$$", " / ");
        }

        return role;
    }

    public static (string part1, string part2) SplitByLastSpace(string? content)
    {
        var result = (string.Empty, string.Empty);

        if (string.IsNullOrWhiteSpace(content)) return result;

        //替换tab为空格
        content = content.Trim().Replace(Constants.TAB_SPACE, Constants.SPACE);

        if (!content.Contains(Constants.SPACE)) return (content, string.Empty);

        var lastSpaceIndex = content.LastIndexOf(Constants.SPACE, StringComparison.Ordinal);
        if (lastSpaceIndex >= 0)
        {
            var part1 = content.Substring(0, lastSpaceIndex);
            var part2 = content.Substring(lastSpaceIndex + 1);

            return (part1, part2);
        }

        return result;
    }

    public static bool IsValidPostion(string? content)
    {
        if (string.IsNullOrWhiteSpace(content)) return false;

        return IsChineseString(RemoveSpecificChars(content));
    }

    public static string RemoveZeroWidthChars(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        
        // 定义需要移除的零宽度字符
        HashSet<char> zeroWidthChars = new HashSet<char> { '\u200B', '\u200C', '\u200D', '\uFEFF', '\u8023' };
        
        // 过滤并构建新字符串
        return new string(input.Where(c => !zeroWidthChars.Contains(c)).ToArray());
    }
    
    public static string RemoveSpecificChars(string? content)
    {
        if (string.IsNullOrWhiteSpace(content)) return content;
        
        return content
            .Replace(Constants.LEFT_PARENTHESIS_EN, string.Empty)
            .Replace(Constants.RIGHT_PARENTHESIS_EN, string.Empty)
            .Replace(Constants.COMMA_EN, string.Empty)
            .Replace(Constants.LEFT_PARENTHESIS_CN, string.Empty)
            .Replace(Constants.RIGHT_PARENTHESIS_CN, string.Empty)
            .Replace(Constants.COMMA_CN, string.Empty);
    }

    public static bool IsChineseString(string? content)
    {
        if (string.IsNullOrWhiteSpace(content)) return false;

        foreach (char c in content)
        {
            // 中文字符的Unicode范围
            if (c < 0x4e00 || c > 0x9fa5)
            {
                return false;
            }
        }

        return true;
    }
}