namespace TagAC.BuildingBlocks.Authorization.JWT
{
    public class JwtConfigSettings
    {
        public string Secret { get; set; }
        public int ExpiresInHours { get; set; }
        public string Issuer { get; set; }
        public string ValidOn { get; set; }
    }
}
