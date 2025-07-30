using System.ComponentModel.DataAnnotations;

namespace BooksCrudApi.Models.DTOs
{
    public class BookRequestDto
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string ISBN { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Publisher { get; set; } = string.Empty;
        
        [Range(1800, 2100)]
        public int PublicationYear { get; set; }
        
        [Range(1, 10000)]
        public int PageCount { get; set; }
        
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string Language { get; set; } = string.Empty;
        
        [Range(0, 10000)]
        public decimal Price { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        [StringLength(500)]
        public string CoverImageUrl { get; set; } = string.Empty;
    }
} 