using System.ComponentModel.DataAnnotations;

namespace WebApiBrowler.Dtos.Response
{
    public class AssetStatusDtoResponse
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
