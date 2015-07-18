using DomainModel;

namespace DomainServices
{
    public interface IExerciseRepository : IRepository<Exercise, long>
    {
        Exercise GetByAlternateName(string name);
    }
}