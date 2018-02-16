namespace WebApiBrowler.Dtos
{
    public partial class Responses
    {
        public class TokenDto
        {
            public string Id { get; set; }
            public string AuthToken { get; set; }
            public int ExpiresIn { get; set; }
        }
    }
}
