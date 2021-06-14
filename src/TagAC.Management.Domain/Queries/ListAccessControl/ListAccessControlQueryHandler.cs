using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TagAC.Management.Domain.Interfaces;

namespace TagAC.Management.Domain.Queries.ListAccessControl
{
    public class ListAccessControlQueryHandler : IRequestHandler<ListAccessControlQuery, ListAccessControlQueryResponse>
    {
        private readonly IAccessControlRepository _repository;
        public ListAccessControlQueryHandler(IAccessControlRepository accessControlRepository)
        {
            _repository = accessControlRepository;
        }

        public async Task<ListAccessControlQueryResponse> Handle(ListAccessControlQuery request, CancellationToken cancellationToken)
        {
            var accessControlList = await _repository.ListAll();

            return new ListAccessControlQueryResponse()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = accessControlList
            };
        }
    }
}
