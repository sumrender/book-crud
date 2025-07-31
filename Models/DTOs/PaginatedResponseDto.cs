using System.Text.Json.Serialization;

namespace BooksCrudApi.Models.DTOs
{
    public class PaginatedResponseDto<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        
        public int TotalCount { get; set; }
        
        public int PageNumber { get; set; }
        
        public int PageSize { get; set; }
        
        public int TotalPages { get; set; }
        
        public bool HasPreviousPage => PageNumber > 1;
        
        public bool HasNextPage => PageNumber < TotalPages;
        
        [JsonIgnore]
        public int Skip => (PageNumber - 1) * PageSize;
        
        [JsonIgnore]
        public int Take => PageSize;
    }
} 