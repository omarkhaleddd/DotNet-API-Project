using System.ComponentModel.DataAnnotations.Schema;

namespace Pokimon.Models
{
    public class Review
    {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public Reviewer Reviewer { get; set; }
    public Pokemon Pokemon { get; set; }
    public String? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile Image { get; set; }
    }
}
