namespace QuestionnarieApp_.Models
{
    public class Message
    {
        public int Id { get; set; } // Unique identifier
        public string SenderId { get; set; } // Sender's user ID (either a user or an admin)
        public string ReceiverId { get; set; } // Receiver's user ID (either a user or an admin)
        public string Content { get; set; } // Message content
        public DateTime SentAt { get; set; } // Timestamp
        public bool IsAdminMessage { get; set; } // Whether the message was sent by an admin
    }

}
