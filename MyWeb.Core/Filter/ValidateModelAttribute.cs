using Microsoft.AspNetCore.Mvc.Filters;
using System.Runtime.Serialization;

namespace MyApp.Web.Filter
{
    public class ModelValidationException : ArgumentException
    {
        public IEnumerable<object> Errors { get; }

        public ModelValidationException(IEnumerable<object> errors)
            : base("Invalid model")
        {
            Errors = errors;
        }
    }

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Select(p => new
                {
                    Field = p.Key,
                    Errors = p.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                }).ToArray();

                throw new ModelValidationException(errors);

            }
        }
    }
}
