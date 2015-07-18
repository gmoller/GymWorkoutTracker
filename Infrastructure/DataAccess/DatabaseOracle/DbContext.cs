using System;
using System.Data;
using Oracle.DataAccess.Client;

namespace DatabaseOracle
{
    public class DbContext
    {
        private const string ConnectionString = "user id=businessdata_ateam4; password=businessdata_ateam4; Data Source=DEV";

        private readonly OracleConnection _dbConnection = new OracleConnection(ConnectionString);
        private OracleTransaction _transaction;

        public OracleConnection DbConnection
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