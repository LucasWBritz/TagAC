using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using TagAC.BuildingBlocks.Authorization;
using TagAC.Domain.Commands;
using TagAC.Management.Domain.Commands.GrantAccess;
using TagAC.Management.Domain.Commands.RevokeAccess;
using TagAC.Management.Domain.Queries.ListAccessControl;

namespace TagAC.Apis.Management.Controllers
{
    [Route("[controller]")]
    [RoleAuthorization("Admin")] // Using the role sent via claims for a basic authentication.
    public class AccessControlController
    {
        private readonly ISender _mediatorSender;
        public AccessControlController(ISender sender)
        {
            _mediatorSender = sender;
        }        

        [HttpGet()]
        public async Task<IActionResult> ListAll([FromBody]ListAccessControlQuery query)
        {
            return CreateQueryResponse(await _mediatorSender.Send(query));
        }

        private IActionResult CreateQueryResponse<TQueryData>(QueryResponse<TQueryData> response)
        {

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new OkObjectResult(response.Data);
            }

            return new BadRequestObjectResult(response);
        }

        private IActionResult CreateCommandResponse(CommandResponse response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return new OkResult();
            }

            return new BadRequestObjectResult(response);
        }

        [HttpPost("grant")]
        public async Task<IActionResult> GrantAccess([FromBody]GrantAccessCommand command, CancellationToken cancellationToken)
        {
            return CreateCommandResponse(await _mediatorSender.Send(command, cancellationToken));
        }

        [HttpDelete("revoke")]
        public async Task<IActionResult> RevokeAccess([FromBody]RevokeAccessCommand command, CancellationToken cancellationToken)
        {
            return CreateCommandResponse(await _mediatorSender.Send(command, cancellationToken));
        }        
    }
}
