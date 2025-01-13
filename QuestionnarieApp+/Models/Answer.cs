namespace QuestionnarieApp_.Models
{
    public class Answer
    {
        public int Id { get; set; } // Unique identifier for the answer
        public string UserId { get; set; } // ID of the user who submitted the answer
        public int OptionId { get; set; } // Selected option ID
        public int SubmissionId { get; set; } // Foreign key to the submission

        public Option Option { get; set; } // Navigation property
        public Submission Submission { get; set; }
    }

}
