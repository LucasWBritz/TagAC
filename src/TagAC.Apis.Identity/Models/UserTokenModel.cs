using System.Collections.Generic;

namespace TagAC.Apis.Identity.Models
{
    public class UserTokenModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaimModel> Claims { get; set; }
    }
}
