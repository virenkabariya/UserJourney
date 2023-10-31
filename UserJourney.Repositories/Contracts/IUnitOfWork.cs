using System.Threading.Tasks;

namespace UserJourney.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        IGenericRepository<T> GetRepository<T>() where T : class;
        Task SaveAsync();
    }
}
