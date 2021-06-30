using System;
using System.Collections.Generic;

namespace SampleEntityFrameworkCore.Models.DbFirst
{
    public partial class Blog
    {
        public long BlogId { get; set; }
        public string Url { get; set; }
    }
}
