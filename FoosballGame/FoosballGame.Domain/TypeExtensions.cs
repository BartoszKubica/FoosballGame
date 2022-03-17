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
                return Team.TeamTwo;

            throw new ArgumentOutOfRangeException(nameof(set), "No winner reachable");
        }
    }
}
