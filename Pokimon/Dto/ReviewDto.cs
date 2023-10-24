namespace Pokimon.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile Image { get; set; }
    }
}
