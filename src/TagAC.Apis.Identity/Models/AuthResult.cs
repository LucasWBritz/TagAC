using System.Collections.Generic;

namespace TagAC.Apis.Identity.Models
{
    public class AuthResult<TResponseValue>
    {
        public AuthResult(TResponseValue response)
        {
            Response = response;
        }

        public ICollection<string> ValidationErrors = new List<string>();
        public TResponseValue Response { get; set; }

        public bool IsValid => ValidationErrors.Count == 0;

        public void AddError(string error)
        {
            ValidationErrors.Add(error);
        }
    }
}
