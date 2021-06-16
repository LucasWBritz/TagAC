using System.Threading.Tasks;
using TagAC.Apis.Identity.Models;

namespace TagAC.Apis.Identity.Services
{
    public interface IAuthenticationService
    {
        Task<AuthResult<LoginResponseModel>> Login(LoginModel input);
        Task<AuthResult<LoginResponseModel>> RegisterUser(RegisterUserModel input);
    }
}
