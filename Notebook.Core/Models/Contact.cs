using System;

namespace Notebook.Core.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastEditDate { get; set; }
        public int UserId { get; set; }
    }
}
