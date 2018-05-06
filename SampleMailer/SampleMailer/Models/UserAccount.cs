using SQLite.Net.Attributes;
using System;

namespace SampleMailer.Models
{
    public class UserAccount
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
