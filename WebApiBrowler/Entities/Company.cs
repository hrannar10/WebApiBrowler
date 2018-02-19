using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace WebApiBrowler.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [NotMapped]
        public List<Guid> UserIds { get; set; } = new List<Guid>();

        [Obsolete("Only for Persistence by EntityFramework")]
        public string IdentiyIdsJsonFromDb
        {
            get { return UserIds == null || !UserIds.Any() ? null : JsonConvert.SerializeObject(UserIds); }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    UserIds.Clear();
                }
                else
                {
                    UserIds = JsonConvert.DeserializeObject<List<Guid>>(value);
                }
            }
        }
    }
}
