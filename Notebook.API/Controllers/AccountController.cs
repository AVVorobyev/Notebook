using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notebook.API.DTO;
using Notebook.Core;
using System.Threading.Tasks;

namespace Notebook.API.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger _logger;

        public AccountController(UserRepository userRepository, ILogger<AccountController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }


        [HttpPost("/registration")]
        public async Task<IActionResult> AddUserAsync(UserDTORequest userDTO)
        {           
            var _user = userDTO.ConvertToUser();
            var _response = await _userRepository.AddUserAsync(_user);

            _logger.LogInformation($">>>>>>>>>> call AddUserAsync");

            return Json(UserDTOResponse.ConvertFronDomainResult(_response));
        }

        [HttpPost("/token")]
        public async Task<IActionResult> GetTokenAsync(UserDTORequest userDTO)
        {
            var _user = userDTO.ConvertToUser();
            var _response = await _userRepository.GetTokenAsync(_user);

            _logger.LogInformation($">>>>>>>>>> call GetTokenAsync");

            return Json(UserDTOResponse.ConvertFronDomainResult(_response));
        }
    }
}

