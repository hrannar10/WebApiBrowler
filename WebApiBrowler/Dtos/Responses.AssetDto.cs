using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiBrowler.Dtos
{
    public partial class Responses
    {
        public class AssetDto
        {
            [Key]
            public Guid Id { get; set; }
            public string CreatedBy { get; set; }
            public DateTime Created { get; set; }
            public string ModifiedBy { get; set; }
            public DateTime Modified { get; set; }
            [Required]
            public string Name { get; set; }
            public string Description { get; set; }
            [DefaultValue(0)]
            public int TypeId { get; set; }
            [DefaultValue(0)]
            public int StatusId { get; set; }
        }
    }
}
