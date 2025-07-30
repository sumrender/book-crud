using System.ComponentModel.DataAnnotations;

namespace BooksCrudApi.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        
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
        
        public int PublicationYear { get; set; }
        
        public int PageCount { get; set; }
        
        [StringLength(50)]
        public string Genre { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string Language { get; set; } = string.Empty;
        
        public decimal Price { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedOn { get; set; }
        
        [StringLength(500)]
        public string CoverImageUrl { get; set; } = string.Empty;
    }
} 