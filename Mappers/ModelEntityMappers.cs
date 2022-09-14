using System.Buffers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Http;
using movies.Models;

namespace movies.Mappers
{
    public static class ModelEntityMappers
    {
        public static Entities.Genre ToEntity(this Models.NewGenre genre)
            => new Entities.Genre(genre.Name);

        public static Entities.Actor ToEntity(this Models.NewActor actor)
            => new Entities.Actor()
            {
                Fullname = actor.Fullname,
                Birthdate = actor.Birthdate
            };
        public static Entities.Actor ToEntity(this Models.UpdatedActor actor, Guid id)
            => new Entities.Actor()
            {
                Id = id,
                Fullname = actor.Fullname,
                Birthdate = actor.Birthdate
            };

        public static Entities.Movie ToEntity(this Models.NewMovie movie, 
            IEnumerable<Entities.Actor> actors, 
            IEnumerable<Entities.Genre> genres)
                => new Entities.Movie()
                {
                    Id = Guid.NewGuid(),
                    Title = movie.Title,
                    Description = movie.Description,
                    ReleaseDate = movie.ReleaseDate,
                    Rating = movie.Rating,
                    Actors = actors.ToList(),
                    Genres = genres.ToList()
                };
        public static Entities.Movie ToEntity(this Models.UpdatedMovie movie, 
            IEnumerable<Entities.Actor> actors, 
            IEnumerable<Entities.Genre> genres,Guid id)
                => new Entities.Movie()
                {
                    Id = id,
                    Title = movie.Title,
                    Description = movie.Description,
                    ReleaseDate = movie.ReleaseDate,
                    Rating = movie.Rating,
                    Actors = actors.ToList(),
                    Genres = genres.ToList()
                };
        public static byte[] ToByte(this IFormFile data)
        {
            var ms = new MemoryStream();
            data.CopyTo(ms);
            var fileBytes = ms.ToArray();
            return fileBytes;
        }
    }
}