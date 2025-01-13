using Microsoft.EntityFrameworkCore;
using QuestionnarieApp_.Models;
using QuestionnarieApp_.Repositories.RepositoryInterfaces;
using QuestionnarieApp_.ViewModels;

namespace QuestionnarieApp_.Repositories
{
    public class QuestionnaireRepository : GenericRepository<Questionnaire>, IQuestionnaireRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionnaireRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Questionnaire> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Set<Questionnaire>()
                .Include(q => q.Questions)
                    .ThenInclude(qu => qu.Options)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<IEnumerable<Questionnaire>> GetAllWithDetailsAsync()
        {
            return await _context.Set<Questionnaire>()
                .Include(q => q.Questions)
                    .ThenInclude(qu => qu.Options)
                .ToListAsync();
        }

        public async Task<QuestionnaireViewModel> GetDetailsWithParticipantCountsAsync(int id)
        {
            return await _context.Questionnaires
                .Where(q => q.Id == id)
                .Select(q => new QuestionnaireViewModel
                {
                    Id = q.Id,
                    Title = q.Title,
                    Description = q.Description,
                    ParticipantCount = q.Submissions.Count(),
                    Questions = q.Questions.Select(question => new QuestionViewModel
                    {
                        Id = question.Id,
                        Text = question.Text,
                        Options = question.Options.Select(option => new OptionViewModel
                        {
                            Id = option.Id,
                            Text = option.Text,
                            PraticipantCount = option.Answers.Count() // Count answers for the option
                        }).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
