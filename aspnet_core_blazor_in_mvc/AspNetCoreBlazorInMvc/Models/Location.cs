using System.Collections.Generic;

namespace AspNetCoreBlazorInMvc.Models
{
    public class Prefecture
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<City> Cities { get; set; } = new();
    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int PrefectureId { get; set; }
        public Prefecture? Prefecture { get; set; }
    }
}
