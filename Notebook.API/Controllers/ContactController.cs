using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notebook.API.DTO;
using Notebook.Core;
using System.Threading.Tasks;

namespace Notebook.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/contact")]
    public class ContactController : Controller
    {
        private readonly ContactRepository _contactRepository;
        private readonly ILogger _logger;
        public ContactController(ContactRepository contactRepository, ILogger<ContactController> logger)
        {
            _contactRepository = contactRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetContactsAsync(int skipPages, int takePages) 
        {
            int _userId = UserIdHelper.GetAuthorizedUserId(User);

            var _response = await _contactRepository.GetContactsAsync(_userId, takePages, skipPages);

            _logger.LogInformation($">>>>>>>>>> call GetContactsAsync");

            return Json(ContactDTOResponse.ConvertFromDomaiResult(_response));
        }

        [HttpPost]
        public async Task<IActionResult> AddContactAsync(ContactDTORequest contactDTO)
        {
            var _contact = contactDTO.ConvertToContact();

            int _userId = UserIdHelper.GetAuthorizedUserId(User);

            var _response = await _contactRepository.AddContactAsync(_userId, _contact);

            _logger.LogInformation($">>>>>>>>>> call AddContactAsync");

            return Json(ContactDTOResponse.ConvertFromDomaiResult(_response));
        }

        [HttpPut]
        public async Task<IActionResult> EditContactAsync(ContactDTORequest contactDTO)
        {
            var _contact = contactDTO.ConvertToContact();

            var _response = await _contactRepository.EditContactAsync(_contact);

            _logger.LogInformation($">>>>>>>>>> call EditContactAsync");

            return Json(ContactDTOResponse.ConvertFromDomaiResult(_response));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteContactAsync(ContactDTORequest contactDTO)
        {
            var _contact = contactDTO.ConvertToContact();
            
            var _response = await _contactRepository.DeleteContactAsync(_contact);

            _logger.LogInformation($">>>>>>>>>> call DeleteContactAsync");

            return Json(ContactDTOResponse.ConvertFromDomaiResult(_response));
        }
    }
}
