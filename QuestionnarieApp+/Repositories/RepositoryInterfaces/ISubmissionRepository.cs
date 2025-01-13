using QuestionnarieApp_.Models;

namespace QuestionnarieApp_.Repositories.RepositoryInterfaces
{
    public interface ISubmissionRepository : IGenericRepository<Submission>
    {
        Task<int> GetSubmissionCountByQuestionnaireIdAsync(int questionnaireId);
        Task<Dictionary<int, int>> GetQuestionnaireSubmissionCountsAsync();
    }
}
