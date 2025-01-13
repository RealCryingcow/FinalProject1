using Microsoft.AspNetCore.Identity;

namespace QuestionnarieApp_.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } 
        public ICollection<Submission> Submissions { get; set; } 
    }
}
