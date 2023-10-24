namespace Pokimon.Models
{
    public class PokimonOwner
    {
        public int PokimonId { get; set; }
        public int OwnerId { get; set; }
        public Pokemon Pokemon { get; set; }
        public Owner Owner { get; set; }


    }
}
