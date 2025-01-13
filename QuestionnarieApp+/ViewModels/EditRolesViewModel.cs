namespace QuestionnarieApp_.ViewModels
{
    public class EditRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<string> SelectedRoles { get; set; } = new List<string>();
    }
}
