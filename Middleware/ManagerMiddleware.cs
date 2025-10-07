using System.Net;
using Newtonsoft.Json;

namespace preparacion_pt_bdv.Middleware
{
    // Middleware para manejar excepciones de forma centralizada.
    public class ManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManagerMiddleware> _logger;

        public ManagerMiddleware(RequestDelegate next, ILogger<ManagerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Captura y maneja centralizadamente las excepciones de la aplicación.
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ManagerExceptionAsync(context, ex, _logger);
            }
        }

        // Convierte una excepción en una respuesta HTTP JSON con el código de estado adecuado.
        private static async Task ManagerExceptionAsync(HttpContext context, Exception exception, ILogger<ManagerMiddleware> logger)
        {
            object? errors = null;
            int statusCode = (int)HttpStatusCode.InternalServerError;

            // Asigna un código de estado según el tipo de excepción.
            switch (exception)
            {
                case MiddlewareException me:
                    statusCode = (int)me.StatusCode;
                    errors = me.Errors;
                    break;
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    break;
                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case Exception e:
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var result = JsonConvert.SerializeObject(new { errors });
            await context.Response.WriteAsync(result ?? string.Empty);
        }
    }
}