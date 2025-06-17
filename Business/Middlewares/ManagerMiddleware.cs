using Business.Services;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Business.Middlewares
{
    public class ManagerMiddleware
    {
        private readonly RequestDelegate _next;

        public ManagerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ManagerProvider managerProvider)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var claimsIdentity = context.User.Identity as ClaimsIdentity;
                var codigoUsuario = claimsIdentity?.FindFirst("codigoUsuario")?.Value;

                if (!string.IsNullOrEmpty(codigoUsuario))
                {
                    managerProvider.SetarUsuario(codigoUsuario);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Usuário não identificado.");
                    return;
                }
            }
            await _next(context);
        }
    }
}
