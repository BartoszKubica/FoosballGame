using FoosballGame.Contracts;
using FoosballGame.Contracts.Queries;
using FoosballGame.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace FoosballGame.WebApi.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMediator mediator;

        public GameController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGame cmd)
        {
            await mediator.Send(cmd);
            return Created(nameof(GetDetails), new { id = cmd.Id});
        }

        [HttpPost("{id}/add-point")]
        public async Task<IActionResult> AddPoint(Guid id, [FromBody] AddPointRequest request)
        {
            await mediator.Send(new AddPointToGame(id, request.Team));
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<GameDetails> GetDetails(Guid id)
            => await mediator.Send<GetGameDetails, GameDetails>(new GetGameDetails(id));

        [HttpGet()]
        public async Task<IReadOnlyCollection<GameDetails>> GetList()
            => await mediator.Send<GetGames, IReadOnlyCollection<GameDetails>>(new GetGames());
    }

    public record AddPointRequest(Team Team);
}