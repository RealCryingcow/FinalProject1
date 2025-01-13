namespace QuestionnarieApp_.Models
{
    public class Submission
    {
        public int Id { get; set; } // Unique identifier for the submission
        public int QuestionnaireId { get; set; } // Foreign key to the questionnaire
        public string UserId { get; set; } // User who submitted the questionnaire
        public DateTime SubmittedAt { get; set; } // Timestamp of submission

        public Questionnaire Questionnaire { get; set; } // Navigation property
        public ICollection<Answer> Answers { get; set; } // Collection of answers
    }

}
