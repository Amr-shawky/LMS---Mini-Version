using LMS___Mini_Version.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LMS___Mini_Version.Attributes
{
    //Any Constructor From The ActionFilter is A Special Type Constructor
    //and Cant Ask the clr to inject an object into a Special Type Ctor
    //if i injected the UOW i will have to inject it in every controller IUnitOfWork unitOfWork
    public class SaveChangesAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executed = await next();
            //we dont want the attribute to catch the error
            //Why?
            //It Would swallow the exception
            if (executed.Exception != null) return;
            if (executed.Result is ObjectResult { StatusCode: >= 400 })
                return;
            //context.HttpContext.RequestAborted if the user abandoned the request closed the tab 
            var unitOfWork = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
            await unitOfWork.SaveChangesAsync();
        }
    }
}
