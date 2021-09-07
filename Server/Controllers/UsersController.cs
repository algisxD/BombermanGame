using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Bomberman.Client.Networking;
using Bomberman.Shared;
using Bomberman.Shared.State_DP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bomberman.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> createUserId()
        {
            if (Startup.FreePlayerSlots.Count > 0)
            {
                Player player = new Player()
                {
                    Position = Startup.FreePlayerSlots.Dequeue(),
                    type = GameObject.Type.Player,
                    Size = 1  
                };
                Startup.GameState.Apply(player);
                player.UserID = player.InstanceID;
                Startup.GameState.AddPlayer(player, Startup.GameState);

                return Ok(new NetworkObject
                {
                    Type = player.type,
                    InstanceID = player.InstanceID,
                    OwnerID = player.UserID,
                    Data = JsonConvert.SerializeObject(player)
                });
            }
            return null;
        }
    }
}
