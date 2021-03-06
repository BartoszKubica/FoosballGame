using FoosballGame.Contracts.Exceptions;

namespace FoosballGame.Domain.AddPoint
{
    // It should return Either<,>, but it's C#
    public static class FoosballGameExtensions
    {
        public static RunningSet CastToRunningSet(this Set set)
            => set.Match((RunningSet running) => running,
                () => throw InvalidState.Create<Set, RunningSet>());

        public static FirstSet CastToFirstSet(this Game game)
            => game.Match((FirstSet firstSet) => firstSet,
                () => throw InvalidState.Create<Game, FirstSet>());

        public static SecondSet CastToSecondSet(this Game game)
            => game.Match((SecondSet secondSet) => secondSet,
                () => throw InvalidState.Create<Game, SecondSet>());

        public static ThirdSet CastToThirdSet(this Game game)
            => game.Match((ThirdSet thirdSet) => thirdSet,
                () => throw InvalidState.Create<Game, ThirdSet>());

        public static FinishedAfterSecondSet CastToFinishedAfterSecondSet(this Game game)
            => game.Match((FinishedGame finishedGame) => finishedGame.Match(
                    (FinishedAfterSecondSet second) => second,
                    () => throw InvalidState.Create<Game, FinishedAfterSecondSet>()),
                () => throw InvalidState.Create<Game, FinishedGame>());

        public static FinishedAfterThirdSet CastToFinishedAfterThirdSet(this Game game)
            => game.Match((FinishedGame finishedGame) => finishedGame.Match(
                    (FinishedAfterThirdSet third) => third,
                    () => throw InvalidState.Create<Game, FinishedAfterSecondSet>()),
                () => throw InvalidState.Create<Game, FinishedGame>());

        public static Game GetResult(this AddPointResult result)
            => result.Visit(game => game,
                matchAlreadyFinished => throw matchAlreadyFinished,
                validationError => throw validationError);
    }
}