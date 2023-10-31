using UserJourney.Repositories.Contracts;
using UserJourney.Repositories.EF;

namespace UserJourney.Repositories
{
    public class ProjectUnitOfWork : UnitOfWork, IProjectUnitOfWork
    {
        public ProjectUnitOfWork(UserJourneyContext context) : base(context)
        {

        }
    }
}
