using Domain.Models;

namespace Domain.Interfaces
{
    public interface IDeveloperRepository: IGenericRepository<Developer>
    {
        public IEnumerable<Developer> GetPopularDevelopers(int count);
    }
}
