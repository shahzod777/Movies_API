using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using movies.Mappers;
using movies.Models;
using movies.Services;

namespace movies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(NewGenre genre)
        {
            if(await _genreService.ExistsAsync(genre.Name))
            {
                return Conflict(nameof(genre.Name));
            }

            var result = await _genreService.CreateAsync(genre.ToEntity());
            
            if(result.IsSuccess)
            {
                return Ok(result.Genre);
            }

            return BadRequest(
            new { 
                isSuccess = false, 
                error = result.Exception.Message 
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            if(await _genreService.ExistsAsync(id))
            {
                return Ok(await _genreService.GetAsync(id));
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
            => Ok(await _genreService.GetAllAsync());

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            try
            {
                var deletedResult = await _genreService.DeleteAsync(id);
                
                if(deletedResult.IsSuccess)
                {
                    return Ok();
                }
                return BadRequest(deletedResult.exception.Message);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}