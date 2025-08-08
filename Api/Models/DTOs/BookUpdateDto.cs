using System.ComponentModel.DataAnnotations;

namespace BooksCrudApi.Models.DTOs
{
    public class BookUpdateDto
    {
        [StringLength(200)]
        public string? Title { get; set; }
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [StringLength(100)]
        public string? Author { get; set; }
        
        [StringLength(50)]
        public string? ISBN { get; set; }
        
        [StringLength(50)]
        public string? Publisher { get; set; }
        
        [Range(1800, 2100)]
        public int? PublicationYear { get; set; }
        
        [Range(1, 10000)]
        public int? PageCount { get; set; }
        
        [StringLength(50)]
        public string? Genre { get; set; }
        
        [StringLength(20)]
        public string? Language { get; set; }
        
        [Range(0, 10000)]
        public decimal? Price { get; set; }
        
        public bool? IsAvailable { get; set; }
        
        [StringLength(500)]
        public string? CoverImageUrl { get; set; }
    }
} 