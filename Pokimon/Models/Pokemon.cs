namespace Pokimon.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<PokimonOwner> PokimonOwners { get; set; }
        public ICollection<PokimonCategory> PokimonCategories { get; set; }

    }
}
