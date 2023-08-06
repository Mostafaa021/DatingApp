using System.Text.Json;
using API.Helpers;

namespace API.Extensions
{
    public static class HttpExtensions 
    {
        public static void AddPaginationHeader(this HttpResponse responce , PaginationHeader header  )
        {
           var jsonOptions =  new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase}; 
           responce.Headers.Add("Pagination",JsonSerializer.Serialize(header,jsonOptions)); // Writing in  Header Responce key:value
           responce.Headers.Add("Access-Control-Expose-Headers","Pagination");  // Allow Access of Pagination to be exposed in headers
        } 
    }
}