using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TagAC.Management.Domain.Commands;
using TagAC.Management.Domain.Commands.GrantAccess;
using TagAC.Management.Domain.Commands.RevokeAccess;

namespace TagAC.Apis.Management.Controllers
{
    [Route("[controller]")]
    public class CredentialsController
    {
        private readonly ISender _mediatorSender;
        public CredentialsController(ISender sender)
        {
            _mediatorSender = sender;
        }

        private IActionResult CreateResponse(CommandResponse response)
        {
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new OkResult();
            }

            return new BadRequestObjectResult(response);
        }

        [HttpPost("grant")]
        public async Task<IActionResult> GrantAccess(GrantAccessCommand command)
        {
            return CreateResponse(await _mediatorSender.Send(command));
        }

        [HttpDelete("revoke")]
        public async Task<IActionResult> RevokeAccess(RevokeAccessCommand command)
        {
            return CreateResponse(await _mediatorSender.Send(command));
        }
    }
}
