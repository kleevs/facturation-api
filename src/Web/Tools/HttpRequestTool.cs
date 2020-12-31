using FacturationApi.Spi;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Web.Tools
{
    public class HttpRequestTool : IHttpRequest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpRequestTool(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public string BaseUrl => _httpContextAccessor.HttpContext.Request?.Headers?.FirstOrDefault(_ => _.Key?.ToLower() == "x-origin").Value ?? string.Empty;
    }
}
