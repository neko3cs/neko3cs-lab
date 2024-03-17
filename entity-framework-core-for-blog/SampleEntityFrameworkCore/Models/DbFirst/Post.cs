using System;
using System.Collections.Generic;

namespace SampleEntityFrameworkCore.Models.DbFirst
{
    public partial class Post
    {
        public long PostId { get; set; }
        public long BlogId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
    }
}
