using Microsoft.AspNetCore.SignalR;
using QuestionnarieApp_.Models;
using QuestionnarieApp_.Repositories.RepositoryInterfaces;
using System.Text.RegularExpressions;

namespace QuestionnarieApp_.Hubs
{
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepository;

        public MessageHub(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        // Send a message from a user to all admins
        public async Task SendMessageToAdmin(string userId, string content)
        {
            try
            {
                Console.WriteLine($"SendMessageToAdmin invoked with UserId: {userId}, Content: {content}");

                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(content))
                {
                    throw new ArgumentException("User ID and content cannot be empty.");
                }

                var message = new Message
                {
                    SenderId = userId,
                    ReceiverId = "Admin",
                    Content = content,
                    SentAt = DateTime.UtcNow,
                    IsAdminMessage = false
                };

                await _messageRepository.AddMessageAsync(message);
                await _messageRepository.SaveChangesAsync();

                await Clients.Group("Admins").SendAsync("ReceiveMessage", userId, content, DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendMessageToAdmin: {ex.Message}");
                throw; // Re-throw the exception to SignalR
            }
        }

        public async Task SendMessageToUser(string adminId, string userId, string content)
        {
            var message = new Message
            {
                SenderId = adminId,
                ReceiverId = userId,
                Content = content,
                SentAt = DateTime.UtcNow,
                IsAdminMessage = true
            };

            await _messageRepository.AddMessageAsync(message);
            await _messageRepository.SaveChangesAsync();

            await Clients.User(userId).SendAsync("ReceiveMessage", adminId, content, DateTime.UtcNow);
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }

            await base.OnConnectedAsync();
        }
    }
}
