using System;

namespace WebApiBrowler.Dtos
{
    public partial class Requests
    {
        public class ModUserCompanyDto
        {
            public int CompanyId { get; set; }
            public Guid UserId { get; set; }
        }
    }
}
