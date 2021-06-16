using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TagAC.Apis.Identity.Models;
using TagAC.BuildingBlocks.Authorization.JWT;

namespace TagAC.Apis.Identity.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private JwtConfigSettings _jwtConfig;       

        public AuthenticationService(ILogger<AuthenticationService>  logger,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IOptions<JwtConfigSettings> jwtSettings)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtConfig = jwtSettings.Value;
        }

        public async Task<AuthResult<LoginResponseModel>> Login(LoginModel input)
        {
            _logger.LogInformation($"Login attempt for {input.Email}");

            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, false, true);

            if (result.Succeeded)
            {
                _logger.LogInformation($"Login succeeded for {input.Email}");

                var jwtToken = await CreateJwt(input.Email);
                return new AuthResult<LoginResponseModel>(jwtToken);
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning($"Account locked for {input.Email}");

                return new AuthResult<LoginResponseModel>(null)
                {
                    ValidationErrors = new string[] { "Account locked." }
                };
            }

            _logger.LogWarning($"Wrong credentials for user {input.Email}");

            return new AuthResult<LoginResponseModel>(null)
            {
                ValidationErrors = new string[] { "Invalid user or password." }
            };
        }

        public async Task<AuthResult<LoginResponseModel>> RegisterUser(RegisterUserModel input)
        {
            _logger.LogInformation($"Registering new user {input.Email}");

            var newUser = new IdentityUser
            {
                UserName = input.Email,
                Email = input.Email,
                EmailConfirmed = true
            };           

            var result = await _userManager.CreateAsync(newUser, input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation($"New user registered. - {input.Email}");

                var jwtResponse = await CreateJwt(newUser.Email);
                return new AuthResult<LoginResponseModel>(jwtResponse);
            }

            _logger.LogWarning($"Error while registering new user {input.Email}. Errors: {string.Concat("-", result.Errors.Select(x => x.Description))}");

            return new AuthResult<LoginResponseModel>(null)
            {
                ValidationErrors = result.Errors.Select(x => x.Description).ToArray()
            };
        }

        private async Task<LoginResponseModel> CreateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await GetUserClaims(claims, user);
            var encodedToken = CreateToken(identityClaims);

            return GetResponseToken(encodedToken, user, claims);
        }

        private LoginResponseModel GetResponseToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
        {
            return new LoginResponseModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_jwtConfig.ExpiresInHours).TotalSeconds,
                Token = new UserTokenModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaimModel { Type = c.Type, Value = c.Value })
                }
            };
        }

        private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string CreateToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.ValidOn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_jwtConfig.ExpiresInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
