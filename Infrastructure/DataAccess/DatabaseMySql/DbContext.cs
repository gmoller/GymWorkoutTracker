using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DatabaseMySql
{
    public class DbContext
    {
        private const string CONNECTION_STRING = "SERVER=localhost;DATABASE=test;UID=gmoller;PASSWORD=Wessels0";

        private readonly MySqlConnection _dbConnection = new MySqlConnection(CONNECTION_STRING);
        private MySqlTransaction _transaction;

        public MySqlConnection DbConnection
        {
            get { return _dbConnection; }
        }

        public bool OpenConnection()
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                _dbConnection.Open();
                return true;
            }

            return false;
        }

        public void CloseConnection()
        {
            if (_dbConnection.State == ConnectionState.Open)
            {
                _dbConnection.Close();
            }
            else
            {
                string message = string.Format("CloseConnection failed. Database connection in state [{0}] must be [{1}].", _dbConnection.State, ConnectionState.Open);
                throw new ApplicationException(message);
            }
        }

        public void BeginTransaction()
        {
            _transaction = _dbConnection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _transaction.Commit();
        }

        public void RollbackTransacion()
        {
            _transaction.Rollback();
        }
    }
}