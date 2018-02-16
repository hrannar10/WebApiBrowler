namespace WebApiBrowler.Dtos.Response
{
    public class TokenDtoResponse
    {
        public string Id { get; set; }
        public string AuthToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
