using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bomberman.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bomberman.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameStateController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetGameState()
        {
            NetworkObject[] toSend = Startup.GameState.gameObjects.Values.Select(g => new NetworkObject
            {
                Type = g.type,
                InstanceID = g.InstanceID,
                OwnerID = g.UserID,
                Data = JsonConvert.SerializeObject(g)
            }).ToArray();

            return Ok(toSend);
        }
    }
}
