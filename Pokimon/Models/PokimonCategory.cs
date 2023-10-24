namespace Pokimon.Models
{
    public class PokimonCategory
    {
        public int PokimonId { get; set; }
        public int CategoryId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Category Category { get; set; }
    }
}
