using Microsoft.EntityFrameworkCore;
using QuestionnarieApp_.Models;

namespace QuestionnarieApp_.Repositories.RepositoryInterfaces
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetMessagesByUserIdAsync(string userId)
        {
            return await _context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllUsersWithMessagesAsync()
        {
            return await _context.Messages
                .Where(m => !m.IsAdminMessage) // Only user-to-admin messages
                .Select(m => m.SenderId)
                .Distinct()
                .ToListAsync();
        }

        public async Task AddMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
