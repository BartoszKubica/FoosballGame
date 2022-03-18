using dotVariant;
using FoosballGame.Contracts;
using FoosballGame.Contracts.Exceptions;

namespace FoosballGame.Domain.AddPoint
{
    public static class AddPointWorkflow
    {
        public static AddPointResult AddPoint(Team team, Game game)
        {
            var result = game.Visit(first =>
                {
                    var set = AddPointInternal(first.State.CastToRunningSet(), team);

                    var newGame = set.Visit(running => Game.Create(first with {State = set}),
                        finished => Game.Create(new SecondSet(Set.Create(Set.InitRunningSet()), finished)));

                    return AddPointResult.Create(newGame);
                }, second =>
                {
                    var set = AddPointInternal(second.State.CastToRunningSet(), team);

                    var newGame = set.Visit(running => Game.Create(second with {State = set}),
                        finished =>
                        {
                            if (finished.Winner == second.FirstSet.Winner)
                                return Game.Create(FinishedGame.Create(new FinishedAfterSecondSet(finished.Winner,
                                    finished.Team1Score, finished.Team2Score)));

                            return Game.Create(new ThirdSet(Set.Create(Set.InitRunningSet()),
                                finished));
                        });

                    return AddPointResult.Create(newGame);
                }, third =>
                {
                    var set = AddPointInternal(third.State.CastToRunningSet(), team);
                    var newGame = set.Visit(running => Game.Create(third with {State = set}),
                        finished => Game.Create(FinishedGame.Create(new FinishedAfterThirdSet(finished.Winner,
                            finished.Team1Score, finished.Team2Score))));

                    return AddPointResult.Create(newGame);
                },
                _ => AddPointResult.Create(MatchIsAlreadyFinished.Create()));

            return result;
        }


        private static Set AddPointInternal(RunningSet set, Team team)
        {
            if (set.Team1Score + 1 == GameConstants.MaxScore)
                return new FinishedSet(Team.TeamOne, GameConstants.MaxScore, set.Team2Score);

            if (set.Team2Score + 1 == GameConstants.MaxScore)
                return new FinishedSet(Team.TeamTwo, set.Team1Score, GameConstants.MaxScore);

            return team switch
            {
                Team.TeamOne => set with {Team1Score = (short) (set.Team1Score + 1)},
                Team.TeamTwo => set with {Team2Score = (short) (set.Team2Score + 1)},
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
            };
        }
    }

    public record FirstSet(Set State);

    public record SecondSet(Set State, FinishedSet FirstSet);

    public record ThirdSet(Set State, FinishedSet SecondSet);

    [Variant]
    public partial class FinishedGame
    {
        static partial void VariantOf(FinishedAfterSecondSet second, FinishedAfterThirdSet third);
    }

    [Variant]
    public partial class Set
    {
        static partial void VariantOf(RunningSet running, FinishedSet finished);

        public static RunningSet InitRunningSet()
            => new(0, 0);
    }

    public record RunningSet(short Team1Score, short Team2Score);

    public record FinishedSet(Team Winner, short Team1Score, short Team2Score);

    public record FinishedAfterSecondSet(Team Winner, short Team1Score, short Team2Score);

    public record FinishedAfterThirdSet(Team Winner, short Team1Score, short Team2Score);

    [Variant]
    public partial class Game
    {
        static partial void VariantOf(FirstSet first, SecondSet second, ThirdSet third, FinishedGame finished);
    }

    [Variant]
    public partial class AddPointResult
    {
        static partial void VariantOf(Game game, MatchIsAlreadyFinished matchAlreadyFinished);
    }
}