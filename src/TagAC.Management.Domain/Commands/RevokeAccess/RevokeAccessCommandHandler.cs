using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Commands;
using TagAC.Domain.Enums;
using TagAC.Management.Domain.Entities;
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
            var currentCredentials = await _repository.GetCredentials(request.RFID, request.SmartLockId);
            if (currentCredentials != null)
            {
                currentCredentials.Status = AuthorizationStatus.Unauthorized;

                _repository.Update(currentCredentials);
            }
            else
            {
                var credentials = new AccessControl()
                {
                    SmartLockId = request.SmartLockId,
                    RFID = request.RFID,
                    Status = AuthorizationStatus.Authorized
                };

                await _repository.CreateAsync(credentials);
            }

            await _repository.UnitOfWork.CommitAsync();

            return new CommandResponse()
            {
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
