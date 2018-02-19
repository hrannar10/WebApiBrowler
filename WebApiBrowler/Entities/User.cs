using System;

namespace WebApiBrowler.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public AppUser Identity { get; set; }  // navigation property
        public string Location { get; set; }
        public string Locale { get; set; }
        public string Gender { get; set; }
    }
}
