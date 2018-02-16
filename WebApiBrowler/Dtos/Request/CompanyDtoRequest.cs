using System.Collections.Generic;

namespace WebApiBrowler.Dtos.Request
{
    public class CompanyDtoRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> IdentityIds { get; set; }
    }
}
