using QuestionnarieApp_.Models;

namespace QuestionnarieApp_.Repositories.RepositoryInterfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesByUserIdAsync(string userId);
        Task<IEnumerable<string>> GetAllUsersWithMessagesAsync();
        Task AddMessageAsync(Message message);
        Task SaveChangesAsync();
    }
}
