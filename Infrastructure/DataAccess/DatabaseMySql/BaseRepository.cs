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
            return GetSingleBy(IdentifierColumn, id, true);
        }

        public T GetSingleBy<T2>(string columnName, T2 value, bool throwExceptionIfNotfound = false)
        {
            string sqlFetch = string.Format("SELECT {0},{1} FROM {2} WHERE {3} = @value1", IdentifierColumn, Columns.Join(","), TableName, columnName);

            List<T> list = GetList(sqlFetch, new List<T2> { value }, throwExceptionIfNotfound);
            if (list.Count > 0)
            {
                return list[0];
            }

            return default(T);
        }

        public List<T> GetListBy<T2>(string columnName, T2 value, bool throwExceptionIfNotfound = false)
        {
            string sqlFetch = string.Format("SELECT {0},{1} FROM {2} WHERE {3} = @value1", IdentifierColumn, Columns.Join(","), TableName, columnName);

            return GetList(sqlFetch, new List<T2> { value }, throwExceptionIfNotfound);
        }

        public List<T> GetListBetween<T2>(string columnName, T2 from, T2 to, bool throwExceptionIfNotfound = false)
        {
            string sqlFetch = string.Format("SELECT {0},{1} FROM {2} WHERE {3} BETWEEN @value1 AND @value2", IdentifierColumn, Columns.Join(","), TableName, columnName);

            return GetList(sqlFetch, new List<T2> { from, to }, throwExceptionIfNotfound);
        }

        public List<T> GetAll()
        {
            string sqlFetch = string.Format("SELECT {0},{1} FROM {2}", IdentifierColumn, Columns.Join(","), TableName);

            return GetList(sqlFetch, new List<T>());
        }

        public List<T> GetList<T2>(string sqlFetch, List<T2> parameters , bool throwExceptionIfNotfound = false)
        {
            bool newConnection = Context.OpenConnection();

            try
            {
                var command = new MySqlCommand(sqlFetch, Context.DbConnection);
                int i = 0;
                foreach (T2 item in parameters)
                {
                    i++;
                    command.Parameters.AddWithValue(string.Format("@value{0}", i), item);
                }
                command.Prepare();
                MySqlDataReader reader = command.ExecuteReader();

                var entityList = new List<T>();
                while (reader.Read())
                {
                    T entity = InstantiateEntityFromReader(reader);
                    entityList.Add(entity);
                }
                reader.Close();

                if (throwExceptionIfNotfound)
                {
                    if (entityList.Count == 0)
                    {
                        throw new ApplicationException("Data not found.");
                    }
                }

                return entityList;
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
            try
            {
                command.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    throw new ApplicationException("Unique key violation.", ex);
                }
                throw;
            }
            

            entity.Id = command.LastInsertedId;

            return entity;
        }

        public IDomainIdentifiable<long> Update(IDomainIdentifiable<long> entity)
        {
            Context.OpenConnection();
            Context.BeginTransaction();

            string sqlUpdate = string.Format("UPDATE {0} SET {1}, date_modified = DEFAULT WHERE {2} = @id", TableName, Columns.BuildSetClause(), IdentifierColumn);
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