using DomainModel;

namespace ApplicationServices
{
    public interface IExerciseService : IBaseService<Exercise, long>
    {
        Exercise GetByAlternateName(string alternateName);
    }
}