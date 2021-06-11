using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using TagAC.Apis.AccessControl.Sessions;

namespace TagAC.Apis.AccessControl.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class AccessControlController
    {
        private readonly IHeaderParametersSession _session;
        private readonly ILogger<AccessControlController> _logger;

        public AccessControlController(IHeaderParametersSession session, ILogger<AccessControlController> logger)
        {
            _session = session;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Authorize()
        {
            _logger.LogInformation($"Request from {_session.RFID} rfid to lock id {_session.LockId}.");
            
            return Authorized();
        }

        private IActionResult Authorized()
        {
            _logger.LogInformation("Authorized");

            return new StatusCodeResult(StatusCodes.Status200OK);
        }

        private IActionResult Unauthorized()
        {
            _logger.LogWarning("Unauthorized. Access denied.");

            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
    }
}
