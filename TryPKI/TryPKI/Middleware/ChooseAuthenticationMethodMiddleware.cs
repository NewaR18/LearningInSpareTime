using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace TryPKI.Middleware
{
    public class ChooseAuthenticationMethodMiddleware
    {
        private readonly RequestDelegate _next;

        public ChooseAuthenticationMethodMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var certificateResult = await context.AuthenticateAsync(CertificateAuthenticationDefaults.AuthenticationScheme);
            if (certificateResult?.Succeeded == true)
            {
                context.User = certificateResult.Principal;
                await _next(context);
                return;
            }

            var jwtResult = await context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (jwtResult?.Succeeded == true)
            {
                context.User = jwtResult.Principal;
                await _next(context);
                return;
            }
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Access Denied");
        }
    }
}
