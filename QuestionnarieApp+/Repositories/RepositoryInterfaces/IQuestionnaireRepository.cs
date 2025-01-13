using QuestionnarieApp_.Models;
using QuestionnarieApp_.ViewModels;

namespace QuestionnarieApp_.Repositories.RepositoryInterfaces
{
    public interface IQuestionnaireRepository : IGenericRepository<Questionnaire>
    {
        Task<Questionnaire> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Questionnaire>> GetAllWithDetailsAsync();
        Task<QuestionnaireViewModel> GetDetailsWithParticipantCountsAsync(int id);
    }

}
