using System.ComponentModel.DataAnnotations;

namespace QuestionnarieApp_.ViewModels
{
    public class QuestionnaireViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Title field is required.")]
        [StringLength(100, ErrorMessage = "The Title must be less than 100 characters.")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "The Description must be less than 500 characters.")]
        public string Description { get; set; }

        [MinLength(1, ErrorMessage = "At least one question is required.")]
        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
        public int ParticipantCount { get; set; } = default(int);
    }
}
