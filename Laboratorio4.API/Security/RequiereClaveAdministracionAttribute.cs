using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Laboratorio4.API.Security;

[AttributeUsage(AttributeTargets.Method)]
public sealed class RequiereClaveAdministracionAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expected = config["Administracion:ApiKey"];
        if (string.IsNullOrWhiteSpace(expected))
        {
            context.Result = new ObjectResult(new { mensaje = "Administración no configurada." }) { StatusCode = 503 };
            return;
        }

        var supplied = context.HttpContext.Request.Headers["X-API-Key"];
        if (supplied.Count != 1 || string.IsNullOrEmpty(supplied[0]))
        {
            context.Result = new UnauthorizedObjectResult(new { mensaje = "Clave de administración requerida." });
            return;
        }

        var actualHash = SHA256.HashData(Encoding.UTF8.GetBytes(supplied[0]!));
        var expectedHash = SHA256.HashData(Encoding.UTF8.GetBytes(expected));
        if (!CryptographicOperations.FixedTimeEquals(actualHash, expectedHash))
        {
            context.Result = new UnauthorizedObjectResult(new { mensaje = "Clave de administración inválida." });
        }
    }
}
