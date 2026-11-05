using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Laboratorio4.API.Security;

// Añade la referencia de seguridad al documento completo para que Swagger UI
// envíe la clave únicamente en las operaciones de administración que escriben.
public sealed class SwaggerClaveAdministracionFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        if (document.Paths is null) return;

        foreach (var path in document.Paths)
        {
            if (!path.Key.StartsWith("/api/administracion/", StringComparison.OrdinalIgnoreCase) ||
                path.Value.Operations is null) continue;

            foreach (var entry in path.Value.Operations)
            {
                if (entry.Key.ToString().ToUpperInvariant() is not ("POST" or "PUT" or "DELETE")) continue;

                entry.Value.Security ??= new List<OpenApiSecurityRequirement>();
                entry.Value.Security.Add(new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("ApiKey", document)] = new List<string>()
                });
            }
        }
    }
}
