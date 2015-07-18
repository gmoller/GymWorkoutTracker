using DomainModel;
using DomainServices;
using MySql.Data.MySqlClient;

namespace DatabaseMySql
{
    public class BodyPartRepository : BaseRepository<BodyPart>, IBodyPartRepository
    {
        protected override string TableName { get { return "muscle_group"; } }
        protected override string IdentifierColumn { get { return "id"; } }
        protected override string[] Columns { get { return new[] { "name" }; } }

        protected override BodyPart InstantiateEntityFromReader(MySqlDataReader reader)
        {
            var entity = new BodyPart
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1)
            };

            return entity;
        }

        protected override void AddCreateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (BodyPart)entity;

            command.Parameters.AddWithValue("@name", e.Name);
        }

        protected override void AddUpdateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (BodyPart)entity;

            command.Parameters.AddWithValue("@name", e.Name);
        }

        public BodyPart GetByName(string name)
        {
            return GetBy("name", name);
        }
    }
}