using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TagAC.Apis.AccessControl.Sessions;

namespace TagAC.Apis.AccessControl.Middlewares
{
    public class HeaderParametersMiddleware
    {
        private readonly RequestDelegate _next;

        public HeaderParametersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IHeaderParametersSession session)
        {
            if(context.Request.Headers.ContainsKey("RFID"))
            {
                var rfid = context.Request.Headers["RFID"].ToString().Trim();

                session.RFID = rfid;
            }

            if (context.Request.Headers.ContainsKey("DeviceId"))
            {
                var lockId = context.Request.Headers["DeviceId"].ToString().Trim();

                session.DeviceId = lockId;
            }

            await _next(context);
        }
    }
}
