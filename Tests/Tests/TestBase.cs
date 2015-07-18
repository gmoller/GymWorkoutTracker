using System.Configuration;
using DataAccess.InMemory;
using DatabaseMySql;
using DomainServices;

namespace Tests
{
    public class TestBase
    {
        protected IBodyPartRepository GetBodyPartRepository()
        {
            string repository = ConfigurationManager.AppSettings["Repository"];

            switch (repository)
            {
                case "InMemory":
                    return new BodyPartInMemoryRepository();
                case "MySql":
                    return new BodyPartRepository();
                case "Oracle":
                    return new BodyPartRepository();
                default:
                    return new BodyPartInMemoryRepository();
            }
        }

        protected ITargetRepository GetTargetRepository()
        {
            string repository = ConfigurationManager.AppSettings["Repository"];

            switch (repository)
            {
                case "InMemory":
                    return new TargetInMemoryRepository();
                case "MySql":
                    return new TargetRepository();
                case "Oracle":
                    return new TargetRepository();
                default:
                    return new TargetInMemoryRepository();
            }
        }

        protected IExerciseRepository GetExerciseRepository()
        {
            string repository = ConfigurationManager.AppSettings["Repository"];

            switch (repository)
            {
                case "InMemory":
                    return new ExerciseInMemoryRepository();
                case "MySql":
                    return new ExerciseRepository();
                case "Oracle":
                    return new ExerciseRepository();
                default:
                    return new ExerciseInMemoryRepository();
            }
        }

        protected IExerciseInstanceRepository GetExerciseInstanceRepository()
        {
            string repository = ConfigurationManager.AppSettings["Repository"];

            switch (repository)
            {
                case "InMemory":
                    return new ExerciseInstanceInMemoryRepository();
                case "MySql":
                    return new ExerciseInstanceRepository();
                case "Oracle":
                    return new ExerciseInstanceRepository();
                default:
                    return new ExerciseInstanceInMemoryRepository();
            }
        }
    }
}