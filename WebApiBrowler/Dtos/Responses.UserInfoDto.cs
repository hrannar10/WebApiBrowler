using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using WebApiBrowler.Entities;

namespace WebApiBrowler.Dtos
{
    public partial class Responses
    {
        public class UserInfoDto
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PictureUrl { get; set; }
            public long? FacebookId { get; set; }
            //public string Location { get; set; }
            //public string Locale { get; set; }
            //public string Gender { get; set; }
            public List<string> Roles { get; set; }

            public Claim[] Claims { get; set; }
        }
    }
}
