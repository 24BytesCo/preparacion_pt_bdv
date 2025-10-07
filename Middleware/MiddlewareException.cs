using System.Net;

namespace preparacion_pt_bdv.Middleware
{
    // Excepción personalizada para manejar errores con códigos de estado HTTP.
    public class MiddlewareException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public object? Errors { get; }

        public MiddlewareException(HttpStatusCode statusCode, object? errors = null)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}