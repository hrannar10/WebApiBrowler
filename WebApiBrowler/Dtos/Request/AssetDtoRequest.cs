using System;
using System.ComponentModel;

namespace WebApiBrowler.Dtos.Request
{
    public class AssetDtoRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [DefaultValue(0)]
        public int TypeId { get; set; }
        [DefaultValue(0)]
        public int StatusId { get; set; }
    }
}
