using DomainModel;
using DomainServices;
using MySql.Data.MySqlClient;

namespace DatabaseMySql
{
    public class ExerciseInstanceRepository : BaseRepository<ExerciseInstance>, IExerciseInstanceRepository
    {
        protected override string TableName { get { return "exercise_instance"; } }
        protected override string IdentifierColumn { get { return "id"; } }
        protected override string[] Columns { get { return new[] { "date", "exercise_id", "set_number", "number_of_reps", "weight" }; } }

        protected override ExerciseInstance InstantiateEntityFromReader(MySqlDataReader reader)
        {
            var exerciseRepository = new ExerciseRepository();

            var entity = new ExerciseInstance
            {
                Id = reader.GetInt64(0),
                Date = reader.GetDateTime(1),
                Exercise = exerciseRepository.Get(reader.GetInt64(2)),
                Set = reader.GetInt32(3),
                Reps = reader.GetInt32(4),
                Weight = reader.GetFloat(5),
            };

            return entity;
        }

        protected override void AddCreateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (ExerciseInstance)entity;

            command.Parameters.AddWithValue("@date", e.Date);
            command.Parameters.AddWithValue("@exercise_id", e.Exercise.Id);
            command.Parameters.AddWithValue("@set_number", e.Set);
            command.Parameters.AddWithValue("@number_of_reps", e.Reps);
            command.Parameters.AddWithValue("@weight", e.Weight);
        }

        protected override void AddUpdateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (ExerciseInstance)entity;

            command.Parameters.AddWithValue("@date", e.Date);
            command.Parameters.AddWithValue("@exercise_id", e.Exercise.Id);
            command.Parameters.AddWithValue("@set_number", e.Set);
            command.Parameters.AddWithValue("@number_of_reps", e.Reps);
            command.Parameters.AddWithValue("@weight", e.Weight);
        }
    }
}