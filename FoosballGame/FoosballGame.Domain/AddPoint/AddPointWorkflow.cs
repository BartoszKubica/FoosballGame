using dotVariant;
using FoosballGame.Contracts;
using FoosballGame.Contracts.Exceptions;

namespace FoosballGame.Domain.AddPoint
{
    public static class AddPointWorkflow
    {
        private const short MaxScore = 10;

        public static Game AddPoint(Team team, Game game)
        {
            return game.Visit(first =>
                {
                    var set = AddPointInternal(first.State.CastToRunningSet(), team);

                    return set.Visit(running => Game.Create(first with {State = set}),
                        finished => Game.Create(new SecondSet(Set.Create(new RunningSet(0, 0)), finished)));
                }, second =>
                {
                    var set = AddPointInternal(second.State.CastToRunningSet(), team);

                    return set.Visit(running => Game.Create(second with {State = set}),
                        finished =>
                        {
                            if (finished.Winner == second.FirstSet.Winner)
                                return Game.Create(new FinishedGame(finished.Winner));
                            return Game.Create(new ThirdSet(Set.Create(new RunningSet(0, 0)),
                                finished));
                        });
                }, third =>
                {
                    var set = AddPointInternal(third.State.CastToRunningSet(), team);
                    return set.Visit(running => Game.Create(third with {State = set}),
                        finished => Game.Create(new FinishedGame(finished.Winner)));
                },
                _ => throw MatchIsAlreadyFinished.Create());
        }

        private static Set AddPointInternal(RunningSet set, Team team)
        {
            if (set.Team1Score + 1 == MaxScore)
                return new FinishedSet(Team.TeamOne);

            if (set.Team2Score + 1 == MaxScore)
                return new FinishedSet(Team.TeamTwo);

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
    public partial class Set
    {
        static partial void VariantOf(RunningSet running, FinishedSet finished);

        public static RunningSet InitRunningSet()
            => new RunningSet(0, 0);
    }

    public record RunningSet(short Team1Score, short Team2Score);

    public record FinishedSet(Team Winner);

    public record FinishedGame(Team Winner);

    [Variant]
    public partial class Game
    {
        static partial void VariantOf(FirstSet first, SecondSet second, ThirdSet third, FinishedGame finished);
    }
}