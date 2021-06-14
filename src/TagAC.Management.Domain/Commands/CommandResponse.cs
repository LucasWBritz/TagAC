using System.Net;

namespace TagAC.Management.Domain.Commands
{
    public class CommandResponse
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
        public string ErrorMessage { get; init; }
    }
}
