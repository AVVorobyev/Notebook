using Notebook.Core;

namespace Notebook.API.DTO
{
    public class UserDTOResponse
    {
        public string Token { get; set; }
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }

        public static UserDTOResponse ConvertFronDomainResult(DomainResult<string> domainResult)
        {
            UserDTOResponse userDTOResponse = new()
            {
                Token = domainResult.Result,
                Succeeded = domainResult.Succeeded,
                ErrorMessage = domainResult.ErrorMessage
            };
            return userDTOResponse;
        }

        public static UserDTOResponse ConvertFronDomainResult(DomainResult domainResult)
        {
            UserDTOResponse userDTOResponse = new()
            {
                Token = null,
                Succeeded = domainResult.Succeeded,
                ErrorMessage = domainResult.ErrorMessage
            };
            return userDTOResponse;
        }
    }
}
