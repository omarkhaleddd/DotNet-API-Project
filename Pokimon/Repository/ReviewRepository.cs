using Microsoft.EntityFrameworkCore;
using Pokimon.Data;
using Pokimon.Interfaces;
using Pokimon.Models;

namespace Pokimon.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _context;
        public ReviewRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReview(Review review)
        {
            await _context.AddAsync(review);
            return await Save();
        }

        public async Task<bool> DeleteReview(Review review)
        {
            _context.Remove(review);
            return await Save();
        }

        public async Task<Review> GetReview(int id)
        {
            return await _context.Reviews.Where(r => r.Id ==id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Review>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsOfPokemon(int pokeId)
        {
                return await _context.Reviews.Where(p => p.Pokemon.Id == pokeId).ToListAsync();
        }

        public async Task<bool> ReviewExists(int id)
        {
            return await _context.Reviews.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
        public async Task<bool> DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return await Save();
        }

        public async Task<bool> UpdateReview(Review review)
        {
            _context.Update(review);
            return await Save();
        }
    }
}
