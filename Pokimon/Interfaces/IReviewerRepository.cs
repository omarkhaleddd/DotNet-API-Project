using Pokimon.Models;

namespace Pokimon.Interfaces
{
    public interface IReviewerRepository
    {
         Task<Reviewer> GetReviewer(int id);
        Task<ICollection<Reviewer>> GetReviewers();
        Task<ICollection<Review>> GetReviewsByReviewer(int id);
        Task<bool> ReviewerExists(int id);
        Task<bool> CreateReviewer(Reviewer reviewer);
        Task<bool> Save();
        Task<bool> DeleteReviewer(Reviewer reviewer);
        Task<bool> UpdateReviewer(Reviewer reviewer);
    }
}
