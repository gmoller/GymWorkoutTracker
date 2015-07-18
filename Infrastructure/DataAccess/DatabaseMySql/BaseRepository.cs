using System;
using System.Collections.Generic;
using System.Text;
using DomainModel;
using MySql.Data.MySqlClient;

namespace DatabaseMySql
{
    public abstract class BaseRepository<T>
    {
        protected readonly DbContext Context = new DbContext();

        protected abstract string TableName { get; }
        protected abstract string IdentifierColumn { get; }
        protected abstract string[] Columns { get; }

        protected abstract T InstantiateEntityFromReader(MySqlDataReader reader);
        protected abstract void AddCreateParams(MySqlCommand command, IDomainIdentifiable<long> entity);
        protected abstract void AddUpdateParams(MySqlCommand command, IDomainIdentifiable<long> entity);

        public T Get(long id)
        {
            return GetBy(IdentifierColumn, id, true);
        }

        public T GetBy<T2>(string columnName, T2 value, bool throwExceptionIfNotfound = false)
        {
            bool newConnection = Context.OpenConnection();

            try
            {
                string sqlFetch = string.Format("SELECT {0},{1} FROM {2} WHERE {3} = @value", IdentifierColumn, Columns.Join(","), TableName, columnName);
                var command = new MySqlCommand(sqlFetch, Context.DbConnection);
                command.Parameters.AddWithValue("@value", value);
                command.Prepare();
                MySqlDataReader reader = command.ExecuteReader();

                T entity = default(T);
                while (reader.Read())
                {
                    entity = InstantiateEntityFromReader(reader);
                }
                reader.Close();

                if (throwExceptionIfNotfound)
                {
                    if (entity == null)
                    {
                        throw new ApplicationException("Data not found.");
                    }
                }

                return entity;
            }
            finally
            {
                if (newConnection)
                {
                    Context.CloseConnection();
                }
            }
        }

        public List<T> GetAll()
        {
            bool newConnection = Context.OpenConnection();

            try
            {
                var allEntities = new List<T>();

                string sqlFetch = string.Format("SELECT {0},{1} FROM {2}", IdentifierColumn, Columns.Join(","), TableName);
                var command = new MySqlCommand(sqlFetch, Context.DbConnection);
                MySqlDataReader reader = command.ExecuteReader();

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
                    Context.CloseConnection();
                }
            }
        }

        public IDomainIdentifiable<long> Create(IDomainIdentifiable<long> entity)
        {
            Context.OpenConnection();
            Context.BeginTransaction();

            string sqlInsert = string.Format("INSERT INTO {0} ({1}) VALUES (@{2})", TableName, Columns.Join(","), Columns.Join(",@"));
            var command = new MySqlCommand(sqlInsert, Context.DbConnection);
            AddCreateParams(command, entity);
            command.Prepare();
            command.ExecuteNonQuery();
            entity.Id = command.LastInsertedId;

            return entity;
        }

        public IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
        {
            Context.OpenConnection();
            Context.BeginTransaction();

            string sqlUpdate = string.Format("UPDATE {0} SET {1} WHERE {2} = @id", TableName, Columns.BuildSetClause(), IdentifierColumn);
            var command = new MySqlCommand(sqlUpdate, Context.DbConnection);
            AddUpdateParams(command, entity);
            AddIdParam(command, entity.Id);
            command.Prepare();
            command.ExecuteNonQuery();

            return entity;
        }

        public void Delete(long id)
        {
            Context.OpenConnection();
            Context.BeginTransaction();

            string sqlDelete = string.Format("DELETE FROM {0} WHERE {1} = @id", TableName, IdentifierColumn);
            var command = new MySqlCommand(sqlDelete, Context.DbConnection);
            AddIdParam(command, id);
            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void Save()
        {
            Context.CommitTransaction();
            Context.CloseConnection();
        }

        private void AddIdParam(MySqlCommand command, long id)
        {
            command.Parameters.AddWithValue("@id", id);
        }
    }

    public static class ExtensionMethods
    {
        public static string Join(this string[] values, string separator)
        {
            return string.Join(separator, values);
        }

        public static string BuildSetClause(this string[] values)
        {
            var sb = new StringBuilder();
            foreach (string s in values)
            {
                sb.Append(string.Format("{0}=@{0},", s));
            }

            return sb.ToString().TrimEnd(',');
        }
    }
}