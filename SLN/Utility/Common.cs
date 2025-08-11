using System.Text.RegularExpressions;

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
}