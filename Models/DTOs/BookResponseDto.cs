namespace BooksCrudApi.Models.DTOs
{
    public class BookResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public int PageCount { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string CoverImageUrl { get; set; } = string.Empty;
    }
} 