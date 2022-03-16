namespace FoosballGame.Contracts.Exceptions
{
    public class MatchIsAlreadyFinished : Exception
    {
        private MatchIsAlreadyFinished() : base("Match is already finished")
        {
        }

        public static MatchIsAlreadyFinished Create()
        {
            return new MatchIsAlreadyFinished();
        }
    }
}
