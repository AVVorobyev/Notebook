using Notebook.Core.Models;
using System;

namespace Notebook.API.DTO
{
    public class ContactDTORequest
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastEditDate { get; set; }
        public int UserId { get; set; }

        public Contact ConvertToContact()
        {
            Contact contact = new()
            {
                Id = Id,
                Number = Number,
                Name = Name,
                Surname = Surname,
                Description = Description,
                CreationDate = CreationDate,
                LastEditDate = LastEditDate,
                UserId = UserId
            };            

            return contact;
        }
    }
}
