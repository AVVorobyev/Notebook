using Notebook.Core.Models;
using System.Collections.Generic;

namespace Notebook.Desktop
{
    internal class ContactDTO
    {
        public IEnumerable<Contact> Contacts { get; set; }
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }
    }
}
