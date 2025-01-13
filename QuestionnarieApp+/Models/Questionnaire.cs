namespace QuestionnarieApp_.Models
{
    public class Questionnaire
    {
        public int Id { get; set; } // Unique identifier for the questionnaire
        public string Title { get; set; } // Title of the questionnaire
        public string Description { get; set; } // Optional description
        public DateTime CreatedAt { get; set; } // Date of creation
        public DateTime? UpdatedAt { get; set; } // Date of the last update
        public QuestionnaireStatus Status { get; set; } // Status of the questionnaire

        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }
}
