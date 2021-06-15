using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Commands;
using TagAC.Domain.Enums;
using TagAC.Management.Domain.Entities;
using TagAC.Management.Domain.Events;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Domain.Commands.GrantAccess
{
    public class GrantAccessCommandHandler : IRequestHandler<GrantAccessCommand, CommandResponse>
    {
        private readonly IAccessControlRepository _repository;
        public GrantAccessCommandHandler(IAccessControlRepository accessCredentialsRepository)
        {
            _repository = accessCredentialsRepository;
        }

        public async Task<CommandResponse> Handle(GrantAccessCommand request, CancellationToken cancellationToken)
        {
            var accessControl = await _repository.GetCredentials(request.RFID, request.SmartLockId);
            if (accessControl != null)
            {
                accessControl.Status = AuthorizationStatus.Authorized;

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

            accessControl.AddEvent(new AccessGrantedEvent(request.SmartLockId, request.RFID));

            await _repository.UnitOfWork.CommitAsync();

            return new CommandResponse()
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
