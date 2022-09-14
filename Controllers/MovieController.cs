using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using movies.Mappers;
using movies.Models;
using movies.Services;
using Newtonsoft.Json;
using movies.Entities;
using Microsoft.AspNetCore.Http;

namespace movies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _ms;
        private readonly IActorService _as;
        private readonly IGenreService _gs;

        public MovieController(
            IMovieService movieService,
            IGenreService genreService,
            IActorService actorService)
        {
            _ms = movieService;
            _as = actorService;
            _gs = genreService;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromForm] NewMovie movie)
        {
            if(movie.ActorIds.Count() < 1 || movie.GenreIds.Count() < 1)
            {
                return BadRequest("Actors and Genres are required");
            }

            if(!movie.GenreIds.All(id => _gs.ExistsAsync(id).Result))
            {
                return BadRequest("Genre doesnt exist");
            }

            if(!movie.ActorIds.All(id => _as.ExistsAsync(id).Result))
            {
                return BadRequest("Actor doesnt exist");
            }

            var genres = movie.GenreIds.Select(id => _gs.GetAsync(id).Result);
            var actors = movie.ActorIds.Select(id => _as.GetAsync(id).Result);
            
            var result = await _ms.CreateAsync(movie.ToEntity(actors, genres));

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
                await _ms.GetAllAsync(), Formatting.Indented,
                new JsonSerializerSettings 
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Ok(json);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute]Guid id)
        {
            var json = JsonConvert.SerializeObject(
                await _ms.GetAsync(id), Formatting.Indented,
                new JsonSerializerSettings 
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            return Ok(json);
        }
        
        [HttpPost]
        [Route("{id}/images")]
        public async Task<IActionResult> PostImagesAsync(Guid id, IEnumerable<IFormFile> files)
        {
            if(!await _ms.ExistsAsync(id))
            {
                return NotFound("Movie with given ID does not exist!");
            }

            var extensions = new List<string>() { ".jpg", ".png", ".svg", ".mp4" };
            var fileSize = 5242880; // 5MB in bytes

            if(files.Count() < 1 || files.Count() > 5)
            {
                return BadRequest("Can upload 1~5 files at a time.");
            }

            // extension validation
            foreach(var file in files)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if(!extensions.Contains(fileExtension.ToString()))
                {
                    return BadRequest($"{fileExtension} format file not allowed!");
                }

                if(file.Length > fileSize)
                {
                    return BadRequest($"Max file size 5MB!");
                }
            }

            var images = files.Select(f => 
            {
                using var stream = new MemoryStream();
                f.CopyTo(stream);

                return new Image()
                {
                    Id = Guid.NewGuid(),
                    Data = stream.ToArray(),
                    MovieId = id
                };
            }).ToList();

            await _ms.CreateImagesAsync(images);

            return Ok();
        }

        [HttpGet("{movieId}/images/{imageId}")]
        public async Task<IActionResult> GetImageAsync(Guid movieId, Guid imageId)
        {
            if(!await _ms.ExistsAsync(movieId) || !await _ms.ImageExistsAsync(imageId))
            {
                return NotFound();
            }

            var image = await _ms.GetImageAsync(imageId);

            return File(new MemoryStream(image.Data), image.ToString());
        }

        [HttpGet("{movieId}/images")]
        public async Task<IActionResult> GetImageAsync(Guid movieId)
        {
            if(!await _ms.ExistsAsync(movieId))
            {
                return NotFound();
            }

            var images = await _ms.GetImagesAsync(movieId);

            return Ok(images.Select(i =>
            {
                return new 
                {
                    ContentType = i.Id,
                    Size = i.Data.Length,
                    Id = i.Id
                };
            }));
        }
    }
}