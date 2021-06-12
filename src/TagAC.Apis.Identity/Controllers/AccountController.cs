using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TagAC.Apis.Identity.Models;
using TagAC.Apis.Identity.Services;

namespace TagAC.Apis.Identity.Controllers
{
    [Route("[controller]")]
    public class AccountController : BaseApiController
    {
        private readonly IAuthenticationService _service;

        public AccountController(IAuthenticationService authenticationService)
        {
            _service = authenticationService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate(LoginModel input)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            return CustomResponse(await _service.Login(input));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount(RegisterUserModel input)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            return CustomResponse(await _service.RegisterUser(input));
        }
    }
}
