using FluentValidation;
using FoosballGame.Contracts;
using FoosballGame.Contracts.Exceptions;
using FoosballGame.Domain.AddPoint;
using Xunit;

namespace FoosballGame.Tests
{
    public class AddPointTests
    {
        [Fact]
        public void AddPoint_ShouldIncreaseScore()
        {
            // Arrange
            var newGame = Game.Create(new FirstSet(
                Set.InitRunningSet()));

            // Act
            var firstSet = AddPointWorkflow.AddPoint(Team.TeamOne, newGame).GetResult()
                .CastToFirstSet().State.CastToRunningSet();

            // Assert
            Assert.Equal(1, firstSet.Team1Score);
            Assert.Equal(0, firstSet.Team2Score);
        }

        [Fact]
        public void AddPoint_WhenTeamReachesTenPoints_ShouldChangeGameToSecondSet()
        {
            // Arrange
            var newGame = Game.Create(new FirstSet(Set.Create(
                new RunningSet(9, 0))));

            // Act
            var game = AddPointWorkflow.AddPoint(Team.TeamOne, newGame).GetResult()
                .CastToSecondSet();
            var secondSet = game.State.CastToRunningSet();

            // Assert
            Assert.Equal(0, secondSet.Team1Score);
            Assert.Equal(0, secondSet.Team2Score);
        }

        [Fact]
        public void AddPoint_WhenTeamWon2Sets_ShouldFinishGame()
        {
            // Arrange
            var newGame = Game.Create(new SecondSet(Set.Create(
                    new RunningSet(9, 0)),
                new FinishedSet(10, 0)));

            // Act
            var game = AddPointWorkflow.AddPoint(Team.TeamOne, newGame).GetResult();

            // Assert
            game.CastToFinishedAfterSecondSet();
        }

        [Fact]
        public void AddPoint_WhenDrawInTwoSets_ShouldStartThirdSet()
        {
            // Arrange
            var newGame = Game.Create(new SecondSet(Set.Create(
                    new RunningSet(0, 9)),
                new FinishedSet(10, 0)));

            // Act
            var game = AddPointWorkflow.AddPoint(Team.TeamTwo, newGame).GetResult()
                .CastToThirdSet();

            // Assert
            var thirdSet = game.State.CastToRunningSet();
            Assert.Equal(0, thirdSet.Team2Score);
            Assert.Equal(0, thirdSet.Team1Score);
        }

        [Fact]
        public void AddPoint_WhenTeamWonInThirdSet_ShouldFinishGame()
        {
            // Arrange
            var newGame = Game.Create(new ThirdSet(new RunningSet(9, 0),
                new FinishedSet(0, 10)));

            // Act
            var game = AddPointWorkflow.AddPoint(Team.TeamTwo, newGame).GetResult();

            // Assert
            game.CastToFinishedAfterThirdSet();
        }

        [Fact]
        public void AddPoint_WhenMatchIsFinished_ShouldReturnMatchIsAlreadyFinished()
        {
            // Arrange
            var newGame = Game.Create(FinishedGame.Create(new FinishedAfterSecondSet(10, 0)));

            // Act
            var result = AddPointWorkflow.AddPoint(Team.TeamTwo, newGame);

            // Assert
            result.Match((MatchIsAlreadyFinished x) => x);
        }

        [Fact]
        public void AddPoint_WhenValidationFailed_ShouldReturnValidationError()
        {
            // Arrange
            var newGame = Game.Create(FinishedGame.Create(new FinishedAfterSecondSet(10, 0)));

            // Act
            var result = AddPointWorkflow.AddPoint(0, newGame);

            // Assert
            result.Match((ValidationException x) => x);
        }
    }
}