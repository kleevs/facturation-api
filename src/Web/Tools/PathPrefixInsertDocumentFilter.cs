using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Web.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class PathPrefixInsertDocumentFilter : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter
    {
        private readonly string _pathPrefix;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        public PathPrefixInsertDocumentFilter(string prefix)
        {
            this._pathPrefix = prefix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths.Keys.ToList();
            foreach (var path in paths)
            {
                var pathToChange = swaggerDoc.Paths[path];
                swaggerDoc.Paths.Remove(path);
                swaggerDoc.Paths.Add(_pathPrefix + path, pathToChange);
            }
        }
    }
}
