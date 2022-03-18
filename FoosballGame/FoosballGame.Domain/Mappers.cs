using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoosballGame.Contracts.Queries;

namespace FoosballGame.Domain
{
    internal static class Mappers
    {
        public static GameDetails ToGameDetails(this GameDb game)
        {
            var setsDetails = new List<SetDetails>
            {
                game.State.FirstSet.ToSetDetails(),
                game.State.SecondSet.ToSetDetails()
            };
            setsDetails.AddIfNotNull(game.State.ThirdSet?.ToSetDetails());
            return new GameDetails(game.Id, game.StartDateTime, setsDetails);
        }

        public static SetDetails ToSetDetails(this SetDb set)
            => new(set.Team1Score, set.Team2Score);
    }
}
