using System.Linq;
using System.Security.Claims;

namespace Notebook.API
{
    public sealed class UserIdHelper
    {        
        public static int GetAuthorizedUserId(ClaimsPrincipal user)
        {
            return int.Parse(user.Claims.First(i => i.Type == "UserId").Value);
        }
    }
}
