using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Notebook.Core.Authorization
{
    public class AuthorizationOptions
    {
        public const string ISSUER = "NotebookServer";
        public const string AUDIENCE = "NotebookClient";
        const string KEY = "kgHHJ621uk231K7d4fwq8W7fP3OBrDCujh";
        public const int LIFETIME = 1;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
