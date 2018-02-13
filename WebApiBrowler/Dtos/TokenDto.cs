namespace WebApiBrowler.Dtos
{
    public class TokenDto
    {
        public string Id { get; set; }
        public string AuthToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
