using FoosballGame.Contracts;
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
            return Created("get", null);
        }
    }
}
