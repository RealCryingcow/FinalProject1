
namespace QuestionnarieApp_.Models
{
    public class Question
    {
        public int Id { get; set; } // Unique identifier for the question
        public int QuestionnaireId { get; set; } // Foreign key to the questionnaire
        public string Text { get; set; } // Text of the question

        public Questionnaire Questionnaire { get; set; } // Navigation property
        public ICollection<Answer> Answers { get; set; } // List of answers for the question
        public ICollection<Option> Options { get; set; } // List of options for multiple-choice
    }

}
