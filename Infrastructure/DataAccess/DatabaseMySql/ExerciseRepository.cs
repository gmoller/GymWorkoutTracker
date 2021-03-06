﻿using DomainModel;
using DomainServices;
using MySql.Data.MySqlClient;

namespace DatabaseMySql
{
    public class ExerciseRepository : BaseRepository<Exercise>, IExerciseRepository
    {
        protected override string TableName { get { return "exercise"; } }
        protected override string IdentifierColumn { get { return "id"; } }
        protected override string[] Columns { get { return new[] { "exrx_name", "alternate_name", "url", "muscle_id" }; } }

        protected override Exercise InstantiateEntityFromReader(MySqlDataReader reader)
        {
            var muscleRepository = new MuscleRepository();

            var entity = new Exercise(reader.GetString(1), reader.GetString(2))
            {
                Id = reader.GetInt64(0),
                Url = reader.GetString(3),
                TargetsMuscle = muscleRepository.Get(reader.GetInt64(4))
            };

            return entity;
        }

        protected override void AddCreateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (Exercise)entity;

            command.Parameters.AddWithValue("@exrx_name", e.ExRxName);
            command.Parameters.AddWithValue("@alternate_name", e.AlternateName);
            command.Parameters.AddWithValue("@url", e.Url);
            command.Parameters.AddWithValue("@muscle_id", e.TargetsMuscle.Id);
        }

        protected override void AddUpdateParams(MySqlCommand command, IDomainIdentifiable<long> entity)
        {
            var e = (Exercise)entity;

            command.Parameters.AddWithValue("@exrx_name", e.ExRxName);
            command.Parameters.AddWithValue("@alternate_name", e.AlternateName);
            command.Parameters.AddWithValue("@url", e.Url);
            command.Parameters.AddWithValue("@muscle_id", e.TargetsMuscle.Id);
        }

        public Exercise GetByAlternateName(string name)
        {
            return GetSingleBy("alternate_name", name);
        }
    }
}