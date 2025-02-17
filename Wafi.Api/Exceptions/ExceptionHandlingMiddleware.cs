// using System.Net;
// using System.Text.Json;
//
// namespace Wafi.SampleTest.Exceptions;
//
// public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
// {
//     public async Task InvokeAsync(HttpContext context)
//     {
//         try
//         {
//             await next(context);
//         }
//         catch (Exception ex)
//         {
//             await HandleExceptionAsync(context, ex);
//         }
//     }
//
//     private async Task HandleExceptionAsync(HttpContext context, Exception exception)
//     {
//         var response = context.Response;
//         response.ContentType = "application/json";
//
//         var (statusCode, message) = exception switch
//         {
//             ValidationException => (HttpStatusCode.BadRequest, exception.Message),
//             NotFoundException => (HttpStatusCode.NotFound, exception.Message),
//             ConflictException => (HttpStatusCode.Conflict, exception.Message),
//             _ => (HttpStatusCode.InternalServerError, "An internal server error occurred.")
//         };
//
//         logger.LogError(exception, "An error occurred: {Message}", exception.Message);
//
//         response.StatusCode = (int)statusCode;
//         var result = JsonSerializer.Serialize(new { message });
//         await response.WriteAsync(result);
//     }
// }
