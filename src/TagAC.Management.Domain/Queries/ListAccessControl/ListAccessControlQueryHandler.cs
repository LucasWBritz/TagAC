using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Domain.Enums;
using TagAC.Management.Domain.Entities;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Domain.Queries.ListAccessControl
{
    public class ListAccessControlQueryHandler : IRequestHandler<ListAccessControlQuery, ListAccessControlQueryResponse>
    {
        private readonly IAccessControlRepository _repository;
        private readonly ISmartLockRepository _smartLockRepository;
        public ListAccessControlQueryHandler(IAccessControlRepository accessControlRepository,
            ISmartLockRepository smartLockRepository)
        {
            _repository = accessControlRepository;
            _smartLockRepository = smartLockRepository;
        }

        public async Task<ListAccessControlQueryResponse> Handle(ListAccessControlQuery request, CancellationToken cancellationToken)
        {
            var accessControlList = await _repository.ListAll(request.RFID);
            var allLocks = await _smartLockRepository.ListAll();

            // Not the best performance, but works for the example.
            // Returns all the locks and checks if we already set the acces for them. If not, then Unauthorized
            var locks = (from l in allLocks
                         let relatedAC = accessControlList.FirstOrDefault(y => y.SmartLockId == l.Id)
                         select new AccessControl()
                         {
                             SmartLockId = l.Id,
                             RFID = request.RFID,
                             Status = relatedAC?.Status ?? AuthorizationStatus.Unauthorized
                         }).ToList();

            return new ListAccessControlQueryResponse()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = locks
            };
        }
    }
}
