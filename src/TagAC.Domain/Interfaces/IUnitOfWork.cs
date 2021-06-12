using System.Threading.Tasks;

namespace TagAC.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
