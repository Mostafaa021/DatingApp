using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _env; 

        public ExceptionMiddleWare(RequestDelegate next ,
         ILogger<ExceptionMiddleWare> logger ,
          IHostEnvironment env )
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
    
     public  async Task InvokeAsync( HttpContext context)
     {
        try 
        {
            await _next(context); // Try to delegate to next middleware
        }
        catch ( Exception ex )
        {
        _logger.LogError( ex , ex.Message); // first log error
        context.Response.ContentType = "application/json"; // error responce type (json)
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError ; // status Code Error be 500
          var response = _env.IsDevelopment()  // if environment is Development or not get responce 
          ? new ApiException(context.Response.StatusCode, ex.Message ,ex.StackTrace.ToString()) 
          : new ApiException(context.Response.StatusCode , ex.Message , "Internal Server Error");
            // error Responce type (json) must have options during selization as Camel Case for Json 
          var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase}; // used by default inside ApiController
          // Serilize responce with options
          var json = JsonSerializer.Serialize(response, options) ;
            // write result in body of context responce
          await context.Response.WriteAsync(json);
        }

     }

    }
}