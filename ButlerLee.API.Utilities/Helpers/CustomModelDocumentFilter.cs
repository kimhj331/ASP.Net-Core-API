using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Threading.Tasks;

namespace ButlerLee.API.Utilities.Helpers
{
    /// <summary>
    /// swagger에 스키마 추가
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CustomModelDocumentFilter<T> : Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter where T : class
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
        }
       
    }
}
