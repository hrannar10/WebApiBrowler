using System.Collections.Generic;

namespace WebApiBrowler.Dtos
{
    public partial class Responses
    {
        public class CompanyDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<string> IdentityIds { get; set; }
        }
    }
}
