using System;

namespace WebApiBrowler.Dtos.Request
{
    public class ModUserCompanyDtoRequest
    {
        public int CompanyId { get; set; }
        public Guid UserId { get; set; }
    }
}
