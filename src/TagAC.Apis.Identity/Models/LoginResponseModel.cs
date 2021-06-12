namespace TagAC.Apis.Identity.Models
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserTokenModel Token { get; set; }
    }
}
