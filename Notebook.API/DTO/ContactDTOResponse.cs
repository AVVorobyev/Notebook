using Notebook.Core;
using Notebook.Core.Models;
using System.Collections.Generic;

namespace Notebook.API.DTO
{
    public class ContactDTOResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public string ErrorMessage { get; set; }


        public static ContactDTOResponse ConvertFromDomaiResult(DomainResult<IEnumerable<Contact>> domainResult)
        {
            ContactDTOResponse contactDTOResponse = new()
            {
                Succeeded = domainResult.Succeeded,
                Contacts = domainResult.Result,
                ErrorMessage = domainResult.ErrorMessage
            };
            return contactDTOResponse;
        }

        public static ContactDTOResponse ConvertFromDomaiResult(DomainResult domainResult)
        {
            ContactDTOResponse contactDTOResponse = new()
            {
                Succeeded = domainResult.Succeeded,
                Contacts = null,
                ErrorMessage = domainResult.ErrorMessage                
            };
            return contactDTOResponse;
        }
    }
}
