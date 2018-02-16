using System.ComponentModel.DataAnnotations;

namespace WebApiBrowler.Dtos
{
    public partial class Responses
    {
        public class AssetStatusDto
        {
            [Key]
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
