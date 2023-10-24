using Pokimon.Models;

namespace Pokimon.Interfaces
{
    public interface IReviewRepository 
    {
        Task<Review> GetReview(int id);
        Task<ICollection<Review>> GetReviews();
        Task<ICollection<Review>> GetReviewsOfPokemon(int pokeId);
        Task<bool> ReviewExists(int id);
        Task<bool> CreateReview(Review review);
        Task<bool> Save();
        Task<bool> DeleteReview(Review review);
        Task<bool> DeleteReviews(List<Review> review);
        Task<bool> UpdateReview(Review review);

    }
}
