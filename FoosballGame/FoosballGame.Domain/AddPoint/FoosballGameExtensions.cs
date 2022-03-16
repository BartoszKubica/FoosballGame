namespace FoosballGame.Domain.AddPoint
{
    public static class FoosballGameExtensions
    {
        public static RunningSet CastToRunningSet(this Set set)
            => set.Match((RunningSet running) => running,
                () => throw new Exception());

        public static FirstSet CastToFirstSet(this Game game)
            => game.Match((FirstSet firstSet) => firstSet,
                () => throw new Exception());

        public static SecondSet CastToSecondSet(this Game game)
            => game.Match((SecondSet secondSet) => secondSet,
                () => throw new Exception());

        public static ThirdSet CastToThirdSet(this Game game)
            => game.Match((ThirdSet thirdSet) => thirdSet,
                () => throw new Exception());

        public static FinishedGame CastToFinishedGame(this Game game)
            => game.Match((FinishedGame finishedGame) => finishedGame,
                () => throw new Exception());
    }
}