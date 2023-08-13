using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class LogUserActivityFilter : IAsyncActionFilter // =>  is used for asynchronous action filters.
      //IActionFilter => is used for synchronous action filters.
    {
        // public void OnActionExecuted(ActionExecutedContext context) //From IActionFilter After Action  Excuted 
        // {
        //     // Code to be executed after the action method is called
        // }

        // public void OnActionExecuting(ActionExecutingContext context)//From IActionFilter  before Action Excuted
        // {
        //    // Code to be executed before the action method is called
        // }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Code to be executed before the action method is called
            
            // Call the next action filter or the action method itself
            var resultContext = await next();
        
            // Code to be executed after the action method is called
            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;
            // get userId which authenticated or logged in
            var userId = resultContext.HttpContext.User.GetUserId();
            // trying to updaed property for user so we gonna access userService
            var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
             var user = await repo.GetUserByIdAsync(userId);
             // update last active property to be just now 
             user.LastActive = DateTime.UtcNow;
             await repo.SaveAllAsync();
             
        }
    }
}