using Db;
using FacturationApi.Api;
using FacturationApi.Spi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Smtp;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Models.Input;
using Web.Tools;

namespace Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IProvider _provider;
        private readonly Logger<AccountsController> _logger;
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly AppConfiguration _appConfiguration;
        private readonly UserWriter _userWriter;

        public AccountsController(
            IAuthenticationProvider authenticationProvider,
            AppConfiguration appConfiguration,
            IProvider provider,
            Logger<AccountsController> logger,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _provider = provider;
            _logger = logger;
            _authenticationProvider = authenticationProvider;
            _appConfiguration = appConfiguration;
            _userWriter = new UserWriter(
                _provider,
                new SmtpClient(_appConfiguration.ConnectionStringSmtp),
                new Encryptor(_appConfiguration.Key, _appConfiguration.Iv),
                new DateTimeService(),
                new HttpRequestTool(httpContextAccessor),
                _logger
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Index()
        {
            var data = await new IdentityManager(_provider, _authenticationProvider).GetUserInfo().ToListAsync();
            var name = await HttpContext.User.Claims
                .Where(_ => _.Type == ClaimTypes.Name)
                .Select(_ => _.Value)
                .FirstOrDefaultAsync();
            var id = await HttpContext.User.Claims
                .Where(_ => _.Type == ClaimTypes.NameIdentifier)
                .Select(_ => _.Value)
                .FirstOrDefaultAsync();

            return new
            {
                id,
                name,
                data
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task Login([FromBody]LoginInputModel form)
        {
            _logger.Info($"tentative de connexion {form.Login} => {form.Password}");
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPut("create")]
        [AllowAnonymous]
        public async Task Create([FromBody]LoginInputModel form) =>
            await _provider.SaveChangesAsync(_userWriter.CreateRequest(form.Login, form.Password));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task Info(int id, [FromBody]UserInputModel form) =>
            await _provider.SaveChangesAsync(_userWriter.Save(id, form));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> Create(string token)
        {
            await _provider.SaveChangesAsync(_userWriter.Create(token));
            return Redirect("/");
        }
    }
}