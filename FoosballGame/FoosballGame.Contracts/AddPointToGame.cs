using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoosballGame.Infrastructure;

namespace FoosballGame.Contracts
{
    public record AddPointToGame(Guid Id, Team Team) : ICommand;

}
