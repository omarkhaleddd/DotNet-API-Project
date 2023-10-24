using Microsoft.EntityFrameworkCore;
using Pokimon.Data;
using Pokimon.Interfaces;
using Pokimon.Models;

namespace Pokimon.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _context;

        public ReviewerRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return await Save();
        }

        public async Task<bool> DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return await Save();
        }

        public async Task<Reviewer> GetReviewer(int id)
        {
            return await _context.Reviewers
               .Where(r => r.Id == id)
               .Include(r => r.Reviews)
               .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Reviewer>> GetReviewers()
        {
            return await _context.Reviewers.ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsByReviewer(int id)
        {
            return await _context.Reviews.Where(r=> r.Reviewer.Id == id).ToListAsync();
        }

        public async Task<bool> ReviewerExists(int id)
        {
            return await _context.Reviewers.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return await Save();
        }
    }
}
