using FacturationApi.Models;
using FacturationApi.Spi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Tools
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            var userId = httpContextAccessor.HttpContext?.User?.Claims
                .Where(_ => _.Type == ClaimTypes.NameIdentifier)
                .Select(_ => _.Value)
                .Select(int.Parse)
                .FirstOrDefault();

            var email = httpContextAccessor.HttpContext?.User?.Claims
                .Where(_ => _.Type == ClaimTypes.Name)
                .Select(_ => _.Value)
                .FirstOrDefault();

            Current = userId.HasValue ? new Login
            {
                Id = userId.Value,
                Email = email
            } : null;
        }

        public ILogin Current { get; private set; }

        public async Task SignInAsync(ILogin user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, $"{user.Id}"),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private class Login : ILogin
        {
            public string Email { get; set; }
            public int Id { get; set; }
        }
    }
}
