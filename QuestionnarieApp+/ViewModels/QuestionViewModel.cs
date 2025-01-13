using System.ComponentModel.DataAnnotations;

namespace QuestionnarieApp_.ViewModels
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Question Text field is required.")]
        public string Text { get; set; }

        [MinLength(2, ErrorMessage = "Each question must have at least two options.")]
        public List<OptionViewModel> Options { get; set; } = new List<OptionViewModel>();
    }
}
