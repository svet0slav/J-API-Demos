using RockPapSci.Data;

namespace RockPapSci.Dtos.Play
{
    public static class WinnerResultsMapper
    {
        public static readonly Dictionary<WinnerResult, string> WinnerResultsTexts = new Dictionary<WinnerResult, string>()
        {
            { WinnerResult.FirstWins, "win" },
            { WinnerResult.Equal, "tie" },
            { WinnerResult.SecondWins, "lose" },
            { WinnerResult.NotAvailable, "N/A" }
        };

        public static string ToText(this WinnerResult result)
        {
            return WinnerResultsTexts[result];
        }
    }
}
