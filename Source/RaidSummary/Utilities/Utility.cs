using System.Text.RegularExpressions;

namespace RaidSummary.Utilities
{
    
    public static class Utility
    {
        public static string DefNameWordSeparator(string defName) =>
            Regex.Replace(defName, @"((?<=\p{Ll})\p{Lu})|((?<!\A)\p{Lu}(?>\p{Ll}))", " $0");
    }
}