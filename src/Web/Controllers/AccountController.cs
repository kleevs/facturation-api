using Db;
using FacturationApi.Api;
using FacturationApi.Spi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models.Input;

namespace Web.Controllers
{
    public class AccountsController : ControllerBase
    {
        private readonly Provider _provider;
        private readonly IAuthenticationProvider _authenticationProvider;

        public AccountsController(IAuthenticationProvider authenticationProvider, Provider provider)
        {
            _provider = provider;
            _authenticationProvider = authenticationProvider;
        }

        [HttpGet]
        public async Task<object> Index()
        {
            var data = new IdentityManager(_provider, _authenticationProvider).GetUserInfo();

            var id = await HttpContext.User.Claims
                .ToAsyncEnumerable()
                .Where(_ => _.Type == ClaimTypes.NameIdentifier)
                .Select(_ => _.Value)
                .FirstOrDefault();

            return new
            {
                id,
                data
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Login([FromBody]LoginInputModel form)
        {
            var user = new IdentityManager(_provider, _authenticationProvider).Login(form.Login, form.Password);
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

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}