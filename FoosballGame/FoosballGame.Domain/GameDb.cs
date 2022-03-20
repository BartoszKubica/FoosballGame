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
        RunningSetEnum RunningSet)
    {
        public Game ResolveState()
            => RunningSet switch
            {
                RunningSetEnum.First => Game.Create(
                    new FirstSet(Set.Create(new RunningSet(FirstSet.Team1Score, FirstSet.Team2Score)))),
                RunningSetEnum.Second => Game.Create(
                    new SecondSet(Set.Create(new RunningSet(SecondSet.Team1Score, SecondSet.Team2Score)),
                        new FinishedSet(FirstSet.Team1Score, FirstSet.Team2Score))),
                RunningSetEnum.Third => Game.Create(
                    new ThirdSet(Set.Create(new RunningSet(ThirdSet!.Team1Score, ThirdSet.Team2Score)),
                        new FinishedSet(SecondSet.Team1Score, SecondSet.Team2Score))),
                RunningSetEnum.None => Game.Create(
                    ThirdSet == null
                        ? FinishedGame.Create(new FinishedAfterSecondSet(SecondSet.Team1Score,
                            SecondSet.Team2Score))
                        : FinishedGame.Create(new FinishedAfterThirdSet(ThirdSet.Team1Score,
                            ThirdSet.Team2Score))),
                _ => throw new ArgumentOutOfRangeException()
            };

        public void ApplyState(Game state)
            => state.Visit(first =>
                {
                    var (team1Score, team2Score) = first.State.CastToRunningSet();
                    RunningSet = RunningSetEnum.First;
                    FirstSet = new SetDb(team1Score, team2Score);
                }, second =>
                {
                    var (team1Score, team2Score) = second.State.CastToRunningSet();
                    RunningSet = RunningSetEnum.Second;
                    FirstSet = FirstSet with
                    {
                        Team2Score = second.FirstSet.Team2Score, Team1Score = second.FirstSet.Team1Score
                    };
                    SecondSet = new SetDb(team1Score, team2Score);
                }, third =>
                {
                    var (team1Score, team2Score) = third.State.CastToRunningSet();
                    RunningSet = RunningSetEnum.Third;
                    SecondSet = SecondSet with
                    {
                        Team2Score = third.SecondSet.Team2Score,
                        Team1Score = third.SecondSet.Team1Score
                    };
                    ThirdSet = new SetDb(team1Score, team2Score);
                }, finished =>
                {
                    RunningSet = RunningSetEnum.None;
                    finished.Visit(second =>
                    {
                        var (team1Score, team2Score) = second;
                        SecondSet = SecondSet with
                        {
                            Team2Score = team2Score,
                            Team1Score = team1Score
                        };
                    }, third =>
                    {
                        var (team1Score, team2Score) = third;
                        ThirdSet = ThirdSet! with
                        {
                            Team2Score = team2Score,
                            Team1Score = team1Score
                        };
                    });
                }
            );

        public static FoosballGameStateDb Initialize()
            => new(new SetDb(0, 0), new SetDb(0, 0), null, RunningSetEnum.First);

        public SetDb FirstSet { get; private set; } = FirstSet;
        public SetDb SecondSet { get; private set; } = SecondSet;
        public SetDb? ThirdSet { get; private set; } = ThirdSet;
        public RunningSetEnum RunningSet { get; private set; } = RunningSet;
    }

    public enum RunningSetEnum
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3,
    }
}