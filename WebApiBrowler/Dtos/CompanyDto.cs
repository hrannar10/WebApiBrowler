using System.Collections.Generic;

namespace WebApiBrowler.Dtos
{
    public class CompanyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> IdentityIds { get; set; }
    }
}
