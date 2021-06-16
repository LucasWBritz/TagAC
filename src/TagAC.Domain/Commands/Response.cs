using System.Net;

namespace TagAC.Domain.Commands
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.OK;
        public string ErrorMessage { get; init; }
    }
}
