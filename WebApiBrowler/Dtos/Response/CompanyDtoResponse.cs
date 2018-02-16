using System.Collections.Generic;

namespace WebApiBrowler.Dtos.Response
{
    public class CompanyDtoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> IdentityIds { get; set; }
    }
}
