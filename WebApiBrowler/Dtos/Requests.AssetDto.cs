using System.ComponentModel;

namespace WebApiBrowler.Dtos
{
    public partial class Requests
    {
        public class AssetDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            [DefaultValue(0)]
            public int TypeId { get; set; }
            [DefaultValue(0)]
            public int StatusId { get; set; }
        }
    }
}
