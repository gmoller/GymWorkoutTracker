using DomainModel;
using DomainServices;
using MySql.Data.MySqlClient;

namespace DatabaseMySql
{
    public class MuscleGroupRepository : BaseRepository<MuscleGroup>, IMuscleGroupRepository
    {
        protected override string TableName { get { return "muscle_group"; } }
        protected override string IdentifierColumn { get { return "id"; } }
        protected override string[] Columns { get { return new[] { "name" }; } }

        protected override MuscleGroup InstantiateEntityFromReader(MySqlDataReader reader)
        {
            var entity = new MuscleGroup(reader.GetString(1))
            {
                Id = reader.GetInt64(0)
            };

            return entity;
        }

        protected override void AddCreateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (MuscleGroup)entity;

            command.Parameters.AddWithValue("@name", e.Name);
        }

        protected override void AddUpdateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (MuscleGroup)entity;

            command.Parameters.AddWithValue("@name", e.Name);
        }

        public MuscleGroup GetByName(string name)
        {
            return GetBy("name", name);
        }
    }
}