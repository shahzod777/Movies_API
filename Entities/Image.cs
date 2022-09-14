using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace movies.Entities
{
    public class Image
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        [Required]
        public byte[] Data { get; set; }
        [Required]
        [ForeignKey("Movies")]
        public Guid MovieId { get; set; }
    }
}