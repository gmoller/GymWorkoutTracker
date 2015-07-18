using DomainModel;
using DomainServices;
using MySql.Data.MySqlClient;

namespace DatabaseMySql
{
    public class TargetRepository : BaseRepository<Target>, ITargetRepository
    {
        protected override string TableName { get { return "muscle"; } }
        protected override string IdentifierColumn { get { return "id"; } }
        protected override string[] Columns { get { return new[] { "name", "muscle_group_id" }; } }

        protected override Target InstantiateEntityFromReader(MySqlDataReader reader)
        {
            var muscleGroupRepository = new MuscleGroupRepository();

            var entity = new Target
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
                MuscleGroup = muscleGroupRepository.Get(reader.GetInt64(2))
            };

            return entity;
        }

        protected override void AddCreateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (Target)entity;

            command.Parameters.AddWithValue("@name", e.Name);
            command.Parameters.AddWithValue("@muscle_group_id", e.MuscleGroup.Id);
        }

        protected override void AddUpdateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (Target)entity;

            command.Parameters.AddWithValue("@name", e.Name);
            command.Parameters.AddWithValue("@muscle_group_id", e.MuscleGroup.Id);
        }

        public Target GetByName(string name)
        {
            return GetBy("name", name);
        }
    }
}