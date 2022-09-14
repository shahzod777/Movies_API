using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using movies.Mappers;
using movies.Models;
using movies.Services;
using Newtonsoft.Json;

namespace movies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _as;

        public ActorController(IActorService actorService)
        {
            _as = actorService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] NewActor actor)
        {
            var result = await _as.CreateAsync(actor.ToEntity());

            if(result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(result.Exception.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var json = JsonConvert.SerializeObject(
                    await _as.GetAllAsync(), Formatting.Indented,
                    new JsonSerializerSettings 
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                return Ok(json);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            if(await _as.ExistsAsync(id))
            {
               var json = JsonConvert.SerializeObject(
                    await _as.GetAsync(id), Formatting.Indented,
                    new JsonSerializerSettings 
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });

                return Ok(json);
            }
            return NotFound();
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            try
            {
                var deletedResult = await _as.DeleteAsync(id);
                
                if(deletedResult.IsSuccess)
                {
                    return Ok();
                }
                return BadRequest(deletedResult.Exception.Message);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> PutAsync([FromRoute]Guid id, [FromForm]UpdatedActor updatedActor)
        {
            if(await _as.ExistsAsync(id))
            {
                var entity = updatedActor.ToEntity(id);
                var updateResult = await _as.UpdateActorAsync(entity);

                if(updateResult.IsSuccess)
                {
                    return Ok(entity);
                }
                return BadRequest(updateResult.Exception.Message);
            }
            return NotFound("There is no actor with given id.");
        }
    }
}