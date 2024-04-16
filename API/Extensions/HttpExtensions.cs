using System.Text.Json;
using API.Helpers;

namespace API.Extensions
{
    public static class HttpExtensions 
    {
        public static void AddPaginationHeader(this HttpResponse responce , PaginationHeader header  )
        {
            // options of serlizations 
           var jsonOptions =  new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            // Writing in  Header Responce key:value
            responce.Headers.Add("Pagination",JsonSerializer.Serialize(header,jsonOptions));
            // Allow Access of Pagination to be exposed in headers
            responce.Headers.Add("Access-Control-Expose-Headers","Pagination"); 
            
        } 
    }
}