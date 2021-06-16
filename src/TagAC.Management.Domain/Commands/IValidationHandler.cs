using System.Threading.Tasks;

namespace TagAC.Management.Domain.Commands
{
    public interface IValidationHandler { }

    public interface IValidationHandler<T> : IValidationHandler
    {
        Task<CommandValidationResult> Validate(T request);
    }
}
