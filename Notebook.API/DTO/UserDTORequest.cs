using Notebook.Core.Models;

namespace Notebook.API.DTO
{
    public sealed class UserDTORequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public User ConvertToUser()
        {
            User user = new()
            {
                Id = Id,
                UserName = UserName,
                Password = Password
            };
            return user;
        }
    }
}
