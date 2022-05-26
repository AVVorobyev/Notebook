using Microsoft.IdentityModel.Tokens;
using Notebook.Core.Authorization;
using Notebook.Core.DbContexts;
using Notebook.Core.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Notebook.Core
{
    public class UserRepository
    {
        private readonly PostgreSQLContext _context;
        public UserRepository(PostgreSQLContext context) => _context = context;

        public async Task<DomainResult> AddUserAsync(User user)
        {
            DomainResult _domainResult;

            if (user.UserName == null || user.UserName.Length < 6 ||
                user.Password == null || user.Password.Length < 6) return DomainResult.Fail(
                    "Username or password is less than 6 characters long");

            user.Password = PasswordsUtility.CreatePasswordHash(user.Password);
            try
            {
                if (!_context.Users.Where(u => u.UserName == user.UserName).Any())
                {
                    var a = await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    _domainResult = DomainResult.Success();
                }
                else
                {
                    _domainResult = DomainResult.Fail("A user with this name already exists");
                }
            }
            catch (Exception ex)
            {
                _domainResult = DomainResult.Fail(ex.Message);
            }
            return _domainResult;
        }

        public async Task<DomainResult<string>> GetTokenAsync(User user)
        {
            var _identity = await GetIdentityAsync(user.UserName, user.Password);

            if (_identity == null) return DomainResult<string>.Fail("Invalid Username or Password");

            var _utcNow = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthorizationOptions.ISSUER,
                audience: AuthorizationOptions.AUDIENCE,
                claims: _identity.Claims,
                notBefore: _utcNow,
                expires: _utcNow.Add(TimeSpan.FromMinutes(AuthorizationOptions.LIFETIME)),
                new SigningCredentials(AuthorizationOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJtw = new JwtSecurityTokenHandler().WriteToken(jwt);

            return DomainResult<string>.Success(encodedJtw);
        }

        private async Task<ClaimsIdentity> GetIdentityAsync(string userName, string password)
        {
            var _result = false; User _user = null;

            try
            {
                _user = _context.Users.Single(u => u.UserName == userName);
            }
            catch (Exception)
            {
                throw;
            }

            if (_user == null) return null;

            var userId = _user.Id.ToString();

            _result = await PasswordsUtility.VerifyPasswordAsync(password, _user.Password);

            if (!_result) return null;

            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, _user.UserName),
                new Claim("UserId", userId)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType, userId);

            return claimsIdentity;
        }
    }
}
