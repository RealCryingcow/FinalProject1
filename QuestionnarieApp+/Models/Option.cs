namespace QuestionnarieApp_.Models
{
    public class Option
    {
        public int Id { get; set; } // Unique identifier for the option
        public int QuestionId { get; set; } // Foreign key to the question
        public string Text { get; set; } // Text of the option

        public Question Question { get; set; } // Navigation property
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
