using System.Threading.Tasks;

namespace Pdf
{
    public interface IRazorRenderer
    {
        Task<string> RenderViewAsync<TModel>(string viewName, TModel model);
    }
}
