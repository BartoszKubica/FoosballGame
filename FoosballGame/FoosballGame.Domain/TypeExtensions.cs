using FoosballGame.Contracts;

namespace FoosballGame.Domain
{
    internal static class TypeExtensions
    {
        public static Team GetWinner(this SetDb set)
        {
            if (set.Team2Score == GameConstants.MaxScore)
                return Team.TeamTwo;
            if (set.Team1Score == GameConstants.MaxScore)
                return Team.TeamOne;

            throw new ArgumentOutOfRangeException(nameof(set), "No winner reachable");
        }
    }

    public static class MyExtensions
    {
        public static void AddIfNotNull<TValue>(this IList<TValue> list, TValue? value)
        {
            if (value != null)
                list.Add(value);
        }
    }
}
