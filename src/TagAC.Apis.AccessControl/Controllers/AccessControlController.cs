using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Apis.AccessControl.Services;
using TagAC.Domain.Enums;

namespace TagAC.Apis.AccessControl.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class AccessControlController
    {
        private readonly ILogger<AccessControlController> _logger;
        private readonly IAccessControlService _service;

        public AccessControlController(ILogger<AccessControlController> logger, IAccessControlService accessControlService)
        {
            _logger = logger;
            _service = accessControlService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Authorize(CancellationToken cancellationToken)
        {
            var authorizationStatus = await _service.GetAuthorization(cancellationToken);

            if (authorizationStatus == AuthorizationStatus.Authorized)
            {
                return Authorized();
            }

            return Denied();
        }

        private IActionResult Authorized()
        {
            _logger.LogInformation("Authorized");

            return new StatusCodeResult(StatusCodes.Status200OK);
        }

        private IActionResult Denied()
        {
            _logger.LogWarning("Unauthorized. Access denied.");

            return new StatusCodeResult(StatusCodes.Status401Unauthorized);
        }
    }
}
