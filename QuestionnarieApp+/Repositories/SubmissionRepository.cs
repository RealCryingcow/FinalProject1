using Microsoft.EntityFrameworkCore;
using QuestionnarieApp_.Models;
using QuestionnarieApp_.Repositories.RepositoryInterfaces;

namespace QuestionnarieApp_.Repositories
{
    public class SubmissionRepository : GenericRepository<Submission>, ISubmissionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubmissionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        public async Task<int> GetSubmissionCountByQuestionnaireIdAsync(int questionnaireId)
        {
            return await _context.Submissions
                .Where(s => s.QuestionnaireId == questionnaireId)
                .CountAsync();
        }

        public async Task<Dictionary<int, int>> GetQuestionnaireSubmissionCountsAsync()
        {
            return await _context.Submissions
                .GroupBy(s => s.QuestionnaireId)
                .Select(g => new { QuestionnaireId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.QuestionnaireId, x => x.Count);
        }

    }
}
