using System;
using System.Collections.Generic;
using DomainModel;
using DomainServices;
using Oracle.DataAccess.Client;

namespace DatabaseOracle
{
    public class BodyPartRepository : IBodyPartRepository
    {
        private readonly DbContext _context = new DbContext();

        public BodyPart Get(long id)
        {
            bool newConnection = _context.OpenConnection();

            try
            {
                const string sqlFetch = "SELECT id, name FROM body_part WHERE id = :id";
                var command = new OracleCommand(sqlFetch, _context.DbConnection);
                AddIdParam(command, id);
                command.Prepare();
                OracleDataReader reader = command.ExecuteReader();

                BodyPart entity = null;
                while (reader.Read())
                {
                    entity = InstantiateEntityFromReader(reader);
                }
                reader.Close();

                if (entity == null)
                {
                    throw new ApplicationException("Data not found.");
                }

                return entity;
            }
            finally
            {
                if (newConnection)
                {
                    _context.CloseConnection();
                }
            }
        }

        public List<BodyPart> GetAll()
        {
            bool newConnection = _context.OpenConnection();

            try
            {
                var allEntities = new List<BodyPart>();

                const string sqlFetch = "SELECT id, name FROM body_part";
                var command = new OracleCommand(sqlFetch, _context.DbConnection);
                OracleDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var entity = InstantiateEntityFromReader(reader);
                    allEntities.Add(entity);
                }
                reader.Close();

                return allEntities;
            }
            finally
            {
                if (newConnection)
                {
                    _context.CloseConnection();
                }
            }
        }

        public IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            _context.OpenConnection();
            _context.BeginTransaction();

            // get nextval
            const string sqlGetNextId = "SELECT MAX(id) + 1 FROM body_part";
            var command = new OracleCommand(sqlGetNextId, _context.DbConnection);
            object obj = command.ExecuteScalar();
            entity.Id = Convert.ToInt64(obj);

            // insert
            const string sqlInsert = "INSERT INTO body_part (id, name) VALUES (:id, :name)";
            command = new OracleCommand(sqlInsert, _context.DbConnection);
            AddIdParam(command, entity.Id);
            AddColumnParams(command, (BodyPart)entity);
            command.Prepare();
            command.ExecuteNonQuery();

            return entity;
        }

        public IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
        {
            _context.OpenConnection();
            _context.BeginTransaction();

            const string sqlUpdate = "UPDATE body_part SET name = :name WHERE id = :id";
            var command = new OracleCommand(sqlUpdate, _context.DbConnection);
            AddColumnParams(command, (BodyPart)entity);
            AddIdParam(command, entity.Id);
            command.Prepare();
            command.ExecuteNonQuery();

            return entity;
        }

        public void Delete(long id)
        {
            _context.OpenConnection();
            _context.BeginTransaction();

            const string sqlDelete = "DELETE FROM body_part WHERE id = :id";
            var command = new OracleCommand(sqlDelete, _context.DbConnection);
            AddIdParam(command, id);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void Save()
        {
            _context.CommitTransaction();
            _context.CloseConnection();
        }

        private BodyPart InstantiateEntityFromReader(OracleDataReader reader)
        {
            var entity = new BodyPart
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1)
            };

            return entity;
        }

        private void AddColumnParams(OracleCommand command, BodyPart entity)
        {
            command.Parameters.Add("name", entity.Name);
        }

        private void AddIdParam(OracleCommand command, long id)
        {
            command.Parameters.Add("id", id);
        }

        public BodyPart GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}