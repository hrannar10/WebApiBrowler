namespace WebApiBrowler.Dtos
{
    public partial class Requests
    {
        public class RegistrationDto
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Location { get; set; }
        }
    }
}
