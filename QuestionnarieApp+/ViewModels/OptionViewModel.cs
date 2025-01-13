using System.ComponentModel.DataAnnotations;

namespace QuestionnarieApp_.ViewModels
{
    public class OptionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Option Text field is required.")]
        public string Text { get; set; }
        public int PraticipantCount { get; set; } = default(int);
    }
}
