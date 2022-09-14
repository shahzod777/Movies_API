using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace movies.Models
{
    public class NewMovie
    {   
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }
        
        [Required]
        [Range(0, 10)]
        public double Rating { get; set; }
        
        
        public DateTimeOffset ReleaseDate { get; set; }
        
        public IEnumerable<Guid> GenreIds { get; set; }

        public ICollection<Guid> ActorIds { get; set; }
    }
}