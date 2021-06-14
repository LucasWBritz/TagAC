using MediatR;

namespace TagAC.Management.Domain.Commands
{
    public class Command : IRequest
    {
    }

    public class Command<TResponse> : IRequest<TResponse>
    {

    }
}
