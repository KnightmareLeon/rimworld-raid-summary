using System.Text.RegularExpressions;

namespace RaidSummary.Utilities
{

    public static partial class Utility
    {
    #if RIMWORLD_17
        [GeneratedRegex(@"((?<=\p{Ll})\p{Lu})|((?<!\A)\p{Lu}(?>\p{Ll}))")]
        private static partial Regex DefNameWordRegex();
        public static string DefNameWordSeparator(string defName) => DefNameWordRegex().Replace(defName, " $0");
    #else
        public static string DefNameWordSeparator(string defName) => Regex.Replace(defName, @"((?<=\p{Ll})\p{Lu})|((?<!\A)\p{Lu}(?>\p{Ll}))"," $0");
    #endif
    }
}