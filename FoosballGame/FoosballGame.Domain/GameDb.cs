using FoosballGame.Contracts;
using FoosballGame.Domain.AddPoint;

namespace FoosballGame.Domain
{
    public class GameDb
    {
        public Guid Id { get; }
        public DateTime StartDateTime { get; }
        public FoosballGameStateDb State { get; }

        public GameDb(Guid id, DateTime startDateTime)
        {
            Id = id;
            StartDateTime = startDateTime;
            State = FoosballGameStateDb.Initialize();
        }

        private GameDb()
        {
            // EF
        }
    }

    public record SetDb(short Team1Score, short Team2Score);

    public record FoosballGameStateDb(SetDb FirstSet, SetDb SecondSet, SetDb? ThirdSet,
        RunningSetEnum RunningSet, Team? Winner)
    {
        public Game ResolveState()
            => RunningSet switch
            {
                RunningSetEnum.First => Game.Create(
                    new FirstSet(Set.Create(new RunningSet(FirstSet.Team1Score, FirstSet.Team2Score)))),
                RunningSetEnum.Second => Game.Create(
                    new SecondSet(Set.Create(new RunningSet(SecondSet.Team1Score, SecondSet.Team2Score)),
                        new FinishedSet(FirstSet.GetWinner(), FirstSet.Team1Score, FirstSet.Team2Score))),
                RunningSetEnum.Third => Game.Create(
                    new ThirdSet(Set.Create(new RunningSet(ThirdSet!.Team1Score, ThirdSet.Team2Score)),
                        new FinishedSet(SecondSet.GetWinner(), SecondSet.Team1Score, SecondSet.Team2Score))),
                RunningSetEnum.None => Game.Create(
                    ThirdSet == null
                        ? FinishedGame.Create(new FinishedAfterSecondSet(SecondSet.GetWinner(), SecondSet.Team1Score,
                            SecondSet.Team2Score))
                        : FinishedGame.Create(new FinishedAfterThirdSet(ThirdSet.GetWinner(), ThirdSet.Team1Score,
                            ThirdSet.Team2Score))),
                _ => throw new ArgumentOutOfRangeException()
            };

        public void ApplyState(Game state)
            => state.Visit(first =>
                {
                    var running = first.State.CastToRunningSet();
                    RunningSet = RunningSetEnum.First;
                    FirstSet = new SetDb(running.Team1Score, running.Team2Score);
                }, second =>
                {
                    var running = second.State.CastToRunningSet();
                    RunningSet = RunningSetEnum.Second;
                    FirstSet = FirstSet with
                    {
                        Team2Score = second.FirstSet.Team2Score, Team1Score = second.FirstSet.Team1Score
                    };
                    SecondSet = new SetDb(running.Team1Score, running.Team2Score);
                }, third =>
                {
                    var running = third.State.CastToRunningSet();
                    RunningSet = RunningSetEnum.Third;
                    SecondSet = SecondSet with
                    {
                        Team2Score = third.SecondSet.Team2Score,
                        Team1Score = third.SecondSet.Team1Score
                    };
                    ThirdSet = new SetDb(running.Team1Score, running.Team2Score);
                }, finished =>
                {
                    RunningSet = RunningSetEnum.None;
                    finished.Visit(second =>
                    {
                        Winner = second.Winner;
                        SecondSet = SecondSet with
                        {
                            Team2Score = second.Team2Score,
                            Team1Score = second.Team1Score
                        };
                    }, third =>
                    {
                        Winner = third.Winner;
                        ThirdSet = ThirdSet! with
                        {
                            Team2Score = third.Team2Score,
                            Team1Score = third.Team1Score
                        };
                    });
                }
            );

        public static FoosballGameStateDb Initialize()
            => new(new SetDb(0, 0), new SetDb(0, 0), null, RunningSetEnum.First, null);

        public SetDb FirstSet { get; private set; } = FirstSet;
        public SetDb SecondSet { get; private set; } = SecondSet;
        public SetDb? ThirdSet { get; private set; } = ThirdSet;
        public RunningSetEnum RunningSet { get; private set; } = RunningSet;
        public Team? Winner { get; private set; } = Winner;
    }

    public enum RunningSetEnum
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3,
    }
}