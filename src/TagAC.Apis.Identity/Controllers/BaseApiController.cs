using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using TagAC.Apis.Identity.Models;

namespace TagAC.Apis.Identity.Controllers
{
    [ApiController]
    public class BaseApiController : Controller
    {
        public BaseApiController()
        {

        }

        protected readonly ICollection<string> ErrorMessages = new List<string>();
        protected virtual void AddError(string message) => ErrorMessages.Add(message);
        protected virtual bool HasErrors() => ErrorMessages.Count > 0;
        protected virtual bool IsValid() => !HasErrors();

        protected ActionResult CustomResponse(object result = null)
        {
            if (IsValid())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", ErrorMessages.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AddError(erro.ErrorMessage);
            }

            return CustomResponse();
        }
       

        protected ActionResult CustomResponse<TResult>(AuthResult<TResult> result)
        {
            if (result.IsValid)
            {
                return CustomResponse(result.Response);
            }

            foreach (var error in result.ValidationErrors)
            {
                AddError(error);
            }

            return CustomResponse();
        }
    }
}
