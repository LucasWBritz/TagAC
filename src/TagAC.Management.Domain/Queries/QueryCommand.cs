using MediatR;

namespace TagAC.Management.Domain.Queries
{
    public class QueryCommand : IRequest
    {
    }

    public class QueryCommand<TResponse> : IRequest<TResponse>
    {

    }
}
