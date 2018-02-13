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
        public List<string> IdentityIds { get; set; } = new List<string>();

        [Obsolete("Only for Persistence by EntityFramework")]
        public string IdentiyIdsJsonFromDb
        {
            get { return IdentityIds == null || !IdentityIds.Any() ? null : JsonConvert.SerializeObject(IdentityIds); }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    IdentityIds.Clear();
                }
                else
                {
                    IdentityIds = JsonConvert.DeserializeObject<List<string>>(value);
                }
            }
        }
    }
}
