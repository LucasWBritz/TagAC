using System;
using System.Threading.Tasks;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Domain.Commands.GrantAccess
{
    public class GrantAccessCommandValidator : IValidationHandler<GrantAccessCommand>
    {
        private readonly ISmartLockRepository _smartLockRepository;
        public GrantAccessCommandValidator(ISmartLockRepository smartLockRepository)
        {
            _smartLockRepository = smartLockRepository;
        }

        public async Task<CommandValidationResult> Validate(GrantAccessCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.RFID))
            {
                return CommandValidationResult.Fail("Invalid RFID");
            }

            if (request.SmartLockId == Guid.Empty)
            {
                return CommandValidationResult.Fail("Invalid smart lock id");
            }

            var lockExists = await _smartLockRepository.LockExists(request.SmartLockId);
            if (!lockExists)
            {
                return CommandValidationResult.Fail("Invalid smart lock.");
            }

            return CommandValidationResult.Success;
        }
    }
}
