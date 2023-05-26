namespace TestTaskAspReact.Contract
{
    public class User
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}
