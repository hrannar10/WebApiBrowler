namespace WebApiBrowler.Dtos.Response
{
    public class UserInfoDtoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PictureUrl { get; set; }
        public long? FacebookId { get; set; }
        public string Location { get; set; }
        public string Locale { get; set; }
        public string Gender { get; set; }
    }
}
