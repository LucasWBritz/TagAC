using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Commands;
using TagAC.Domain.Enums;
using TagAC.Management.Domain.Entities;
using TagAC.Management.Domain.Events;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Domain.Commands.RevokeAccess
{
    public class RevokeAccessCommandHandler : IRequestHandler<RevokeAccessCommand, CommandResponse>
    {
        private readonly IAccessControlRepository _repository;

        public RevokeAccessCommandHandler(IAccessControlRepository accessCredentialsRepository)
        {
            _repository = accessCredentialsRepository;
        }

        public async Task<CommandResponse> Handle(RevokeAccessCommand request, CancellationToken cancellationToken)
        {
            var accessControl = await _repository.GetCredentials(request.RFID, request.SmartLockId);
            if (accessControl != null)
            {
                accessControl.Status = AuthorizationStatus.Unauthorized;

                _repository.Update(accessControl);
            }
            else
            {
                accessControl = new AccessControl()
                {
                    SmartLockId = request.SmartLockId,
                    RFID = request.RFID,
                    Status = AuthorizationStatus.Authorized
                };

                await _repository.CreateAsync(accessControl);
            }

            accessControl.AddEvent(new AccessRevokedEvent(request.SmartLockId, request.RFID));

            await _repository.UnitOfWork.CommitAsync();

            return new CommandResponse()
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
