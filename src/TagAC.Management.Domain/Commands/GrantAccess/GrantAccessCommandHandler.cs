using MediatR;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Commands;
using TagAC.Domain.Enums;
using TagAC.Management.Domain.Entities;
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
            var currentCredentials = await _repository.GetCredentials(request.RFID, request.SmartLockId);
            if (currentCredentials != null)
            {
                currentCredentials.Status = AuthorizationStatus.Authorized;

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
