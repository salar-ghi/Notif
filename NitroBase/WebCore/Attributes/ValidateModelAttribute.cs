using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using WebCore.Helpers;

namespace WebCore.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (context.ModelState.IsValid) return;

            var errors = new List<string>();
            foreach (var modelState in context.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                        errors.Add(error.ErrorMessage);
            }

            var result = APIResultHelper.RestResultBody("پارامترهای ورودی صحیح نمی باشند", errors);
            context.Result = new BadRequestObjectResult(result);
        }
    }
}
