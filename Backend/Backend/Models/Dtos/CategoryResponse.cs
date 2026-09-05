namespace Backend.Models.Dtos
{
    public class CategoryResponse
    {
        public int CategoryId { get; set; }

        public string NameAr { get; set; } = string.Empty;

        public string NameEn { get; set; } = string.Empty;

        public string? IconUrl { get; set; }
    }
}
